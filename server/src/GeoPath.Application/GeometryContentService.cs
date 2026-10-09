using GeoPath.Domain;

namespace GeoPath.Application;

/// <summary>Provides read-only access to published geometry content.</summary>
public sealed class GeometryContentService
{
    /// <summary>Provides access to published content records.</summary>
    private readonly IGeometryContentRepository _repository;

    /// <summary>Creates the service with its content repository.</summary>
    /// <param name="repository">Storage used to retrieve published content.</param>
    public GeometryContentService(IGeometryContentRepository repository)
    {
        _repository = repository;
    }

    /// <summary>Lists published examples with their topic and related-content references.</summary>
    /// <param name="cancellationToken">Cancels the query if requested.</param>
    /// <returns>Published example response models.</returns>
    public async Task<IReadOnlyList<ExampleResponse>> GetExamplesAsync(CancellationToken cancellationToken = default)
    {
        var examples = await _repository.GetPublishedExamplesAsync(cancellationToken);
        return examples.Select(ToResponse).ToArray();
    }

    /// <summary>Gets a published example by identifier.</summary>
    /// <param name="id">The example identifier.</param>
    /// <param name="cancellationToken">Cancels the query if requested.</param>
    /// <returns>The example response, or <see langword="null"/> when not found.</returns>
    public async Task<ExampleResponse?> GetExampleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var example = await _repository.GetPublishedExampleAsync(id, cancellationToken);
        return example is null ? null : ToResponse(example);
    }

    /// <summary>Lists published theorems.</summary>
    /// <param name="cancellationToken">Cancels the query if requested.</param>
    /// <returns>Published theorem response models.</returns>
    public async Task<IReadOnlyList<TheoremResponse>> GetTheoremsAsync(CancellationToken cancellationToken = default)
    {
        var theorems = await _repository.GetPublishedTheoremsAsync(cancellationToken);
        return theorems.Select(theorem => new TheoremResponse(
            theorem.Id,
            theorem.Name,
            theorem.Statement,
            theorem.Explanation)).ToArray();
    }

    /// <summary>Lists published definitions.</summary>
    /// <param name="cancellationToken">Cancels the query if requested.</param>
    /// <returns>Published definition response models.</returns>
    public async Task<IReadOnlyList<DefinitionResponse>> GetDefinitionsAsync(CancellationToken cancellationToken = default)
    {
        var definitions = await _repository.GetPublishedDefinitionsAsync(cancellationToken);
        return definitions.Select(definition => new DefinitionResponse(
            definition.Id,
            definition.Name,
            definition.Description)).ToArray();
    }

    /// <summary>Maps a domain example to its public API representation.</summary>
    /// <param name="example">The published geometry example to map.</param>
    /// <returns>The example response with related theorems and definitions.</returns>
    private static ExampleResponse ToResponse(GeometryExample example)
    {
        return new ExampleResponse(
            example.Id,
            example.Title,
            example.Prompt,
            example.GuidedFlowJson,
            example.Difficulty,
            example.TopicId,
            example.Topic?.Name ?? string.Empty,
            example.Theorems.Select(link => new TheoremReference(
                link.TheoremId,
                link.Theorem?.Name ?? string.Empty,
                link.Theorem?.Statement ?? string.Empty)).ToArray(),
            example.Definitions.Select(link => new DefinitionReference(
                link.DefinitionId,
                link.Definition?.Name ?? string.Empty,
                link.Definition?.Description ?? string.Empty)).ToArray());
    }
}

/// <summary>Creates and retrieves attempts owned by the authenticated learner.</summary>
public sealed class LearningAttemptService
{
    /// <summary>Limits optional learner response JSON to 64 KiB per stage.</summary>
    private const int MaximumResponseJsonLength = 65_536;

    /// <summary>Persists and retrieves learner attempts.</summary>
    private readonly ILearningAttemptRepository _attemptRepository;

    /// <summary>Checks that attempts refer to published examples.</summary>
    private readonly IGeometryContentRepository _contentRepository;

    /// <summary>Creates the service with attempt and content repositories.</summary>
    /// <param name="attemptRepository">Storage for learner attempts.</param>
    /// <param name="contentRepository">Storage used to validate published examples.</param>
    public LearningAttemptService(
        ILearningAttemptRepository attemptRepository,
        IGeometryContentRepository contentRepository)
    {
        _attemptRepository = attemptRepository;
        _contentRepository = contentRepository;
    }

    /// <summary>Starts an attempt for a published example on behalf of a learner.</summary>
    /// <param name="learnerId">The learner's authenticated identifier.</param>
    /// <param name="exampleId">The published example identifier.</param>
    /// <param name="cancellationToken">Cancels repository operations if requested.</param>
    /// <returns>The newly persisted attempt.</returns>
    /// <exception cref="InvalidOperationException">The example is not published or does not exist.</exception>
    public async Task<LearningAttempt> CreateAsync(
        Guid learnerId,
        Guid exampleId,
        CancellationToken cancellationToken = default)
    {
        var example = await _contentRepository.GetPublishedExampleAsync(exampleId, cancellationToken);
        if (example is null)
        {
            throw new InvalidOperationException("Published example was not found.");
        }

        return await _attemptRepository.CreateAsync(new LearningAttempt
        {
            LearnerId = learnerId,
            ExampleId = exampleId,
        }, cancellationToken);
    }

    /// <summary>Lists the authenticated learner's attempts.</summary>
    /// <param name="learnerId">The learner's authenticated identifier.</param>
    /// <param name="cancellationToken">Cancels the query if requested.</param>
    /// <returns>The learner's attempts.</returns>
    public Task<IReadOnlyList<LearningAttempt>> GetForLearnerAsync(
        Guid learnerId,
        CancellationToken cancellationToken = default)
    {
        return _attemptRepository.GetByLearnerAsync(learnerId, cancellationToken);
    }

    /// <summary>Finds an attempt only if it belongs to the authenticated learner.</summary>
    /// <param name="attemptId">The attempt identifier.</param>
    /// <param name="learnerId">The learner's authenticated identifier.</param>
    /// <param name="cancellationToken">Cancels the query if requested.</param>
    /// <returns>The owned attempt, or <see langword="null"/> when it is unavailable.</returns>
    public Task<LearningAttempt?> GetForLearnerAsync(
        Guid attemptId,
        Guid learnerId,
        CancellationToken cancellationToken = default)
    {
        return _attemptRepository.GetByIdForLearnerAsync(attemptId, learnerId, cancellationToken);
    }

    /// <summary>Records a result for one stage of an owned, in-progress attempt.</summary>
    /// <param name="attemptId">The attempt identifier.</param>
    /// <param name="learnerId">The learner's authenticated identifier.</param>
    /// <param name="stage">The stage whose outcome is recorded.</param>
    /// <param name="outcome">The learner's outcome for the stage.</param>
    /// <param name="responseJson">Optional JSON containing the learner's response.</param>
    /// <param name="cancellationToken">Cancels repository operations if requested.</param>
    /// <returns>The updated attempt, or <see langword="null"/> when it is not owned by the learner.</returns>
    /// <exception cref="InvalidOperationException">The stage, outcome, or response payload is invalid.</exception>
    public async Task<LearningAttempt?> RecordStageResultAsync(
        Guid attemptId,
        Guid learnerId,
        AttemptStage stage,
        StageOutcome outcome,
        string? responseJson,
        CancellationToken cancellationToken = default)
    {
        if (!Enum.IsDefined(stage) || !Enum.IsDefined(outcome))
        {
            throw new InvalidOperationException("Unsupported stage or outcome.");
        }

        if (responseJson is { Length: > MaximumResponseJsonLength })
        {
            throw new InvalidOperationException("Response payload exceeds the maximum allowed size.");
        }

        if (!string.IsNullOrWhiteSpace(responseJson))
        {
            try
            {
                using var document = System.Text.Json.JsonDocument.Parse(responseJson);
            }
            catch (System.Text.Json.JsonException exception)
            {
                throw new InvalidOperationException("Response payload must contain valid JSON.", exception);
            }
        }

        var attempt = await _attemptRepository.GetByIdForLearnerAsync(attemptId, learnerId, cancellationToken);
        if (attempt is null)
        {
            return null;
        }

        if (attempt.Status != AttemptStatus.InProgress)
        {
            throw new InvalidOperationException("Stage results can only be recorded for in-progress attempts.");
        }

        return await _attemptRepository.SaveStageResultAsync(
            attemptId,
            learnerId,
            new AttemptStageResult
            {
                Stage = stage,
                Outcome = outcome,
                ResponseJson = responseJson,
            },
            cancellationToken);
    }

    /// <summary>Converts a domain attempt to the API response shape.</summary>
    /// <param name="attempt">The attempt to map.</param>
    /// <returns>The attempt and its persisted stage outcomes.</returns>
    public static AttemptResponse ToResponse(LearningAttempt attempt)
    {
        return new AttemptResponse(
            attempt.Id,
            attempt.ExampleId,
            attempt.Status,
            attempt.StartedAtUtc,
            attempt.CompletedAtUtc,
            attempt.StageResults
                .OrderBy(result => result.Stage)
                .Select(result => new AttemptStageResponse(
                    result.Stage,
                    result.Outcome,
                    result.ResponseJson,
                    result.RecordedAtUtc))
                .ToArray());
    }
}
