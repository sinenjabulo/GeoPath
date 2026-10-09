using GeoPath.Domain;

namespace GeoPath.Application;

/// <summary>Describes a published geometry example returned by the API.</summary>
/// <param name="Id">The example's identifier.</param>
/// <param name="Title">The example's display title.</param>
/// <param name="Prompt">The problem statement shown to learners.</param>
/// <param name="GuidedFlowJson">Optional versioned diagram and guided-flow data.</param>
/// <param name="Difficulty">The example's practice difficulty.</param>
/// <param name="TopicId">The containing topic's identifier.</param>
/// <param name="TopicName">The containing topic's display name.</param>
/// <param name="Theorems">The theorem references associated with the example.</param>
/// <param name="Definitions">The definition references associated with the example.</param>
public sealed record ExampleResponse(
    Guid Id,
    string Title,
    string Prompt,
    string? GuidedFlowJson,
    ExampleDifficulty Difficulty,
    Guid TopicId,
    string TopicName,
    IReadOnlyList<TheoremReference> Theorems,
    IReadOnlyList<DefinitionReference> Definitions);

/// <summary>Describes a theorem reference associated with content.</summary>
/// <param name="Id">The theorem's identifier.</param>
/// <param name="Name">The theorem's display name.</param>
/// <param name="Statement">The theorem statement.</param>
public sealed record TheoremReference(Guid Id, string Name, string Statement);

/// <summary>Describes a definition reference associated with content.</summary>
/// <param name="Id">The definition's identifier.</param>
/// <param name="Name">The definition's display name.</param>
/// <param name="Description">The definition's explanatory text.</param>
public sealed record DefinitionReference(Guid Id, string Name, string Description);

/// <summary>Describes a published theorem returned by the API.</summary>
/// <param name="Id">The theorem's identifier.</param>
/// <param name="Name">The theorem's display name.</param>
/// <param name="Statement">The theorem statement.</param>
/// <param name="Explanation">The optional learner-facing explanation.</param>
public sealed record TheoremResponse(Guid Id, string Name, string Statement, string? Explanation);

/// <summary>Describes a published definition returned by the API.</summary>
/// <param name="Id">The definition's identifier.</param>
/// <param name="Name">The definition's display name.</param>
/// <param name="Description">The definition's explanatory text.</param>
public sealed record DefinitionResponse(Guid Id, string Name, string Description);

/// <summary>Contains the example selected to begin a learner attempt.</summary>
/// <param name="ExampleId">The published example the learner will work on.</param>
public sealed record CreateAttemptRequest(Guid ExampleId);

/// <summary>Contains a learner's outcome and optional response for one attempt stage.</summary>
/// <param name="Outcome">The outcome recorded for the thinking stage.</param>
/// <param name="ResponseJson">Optional JSON data describing the learner's response.</param>
public sealed record RecordStageResultRequest(StageOutcome Outcome, string? ResponseJson);

/// <summary>Describes a persisted stage outcome in an attempt response.</summary>
/// <param name="Stage">The thinking stage.</param>
/// <param name="Outcome">The recorded learner outcome.</param>
/// <param name="ResponseJson">The learner's optional response payload.</param>
/// <param name="RecordedAtUtc">When the result was recorded.</param>
public sealed record AttemptStageResponse(
    AttemptStage Stage,
    StageOutcome Outcome,
    string? ResponseJson,
    DateTimeOffset RecordedAtUtc);

/// <summary>Describes a learner's attempt and its recorded stage outcomes.</summary>
/// <param name="Id">The attempt identifier.</param>
/// <param name="ExampleId">The example worked on.</param>
/// <param name="Status">The attempt state.</param>
/// <param name="StartedAtUtc">When the attempt started.</param>
/// <param name="CompletedAtUtc">When the attempt completed, if completed.</param>
/// <param name="StageResults">The outcomes recorded for the attempt's stages.</param>
public sealed record AttemptResponse(
    Guid Id,
    Guid ExampleId,
    AttemptStatus Status,
    DateTimeOffset StartedAtUtc,
    DateTimeOffset? CompletedAtUtc,
    IReadOnlyList<AttemptStageResponse> StageResults);
