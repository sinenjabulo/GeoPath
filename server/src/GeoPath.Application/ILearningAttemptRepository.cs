using GeoPath.Domain;

namespace GeoPath.Application;

/// <summary>Defines persistence operations for learner attempts and their stage results.</summary>
public interface ILearningAttemptRepository
{
    /// <summary>Creates a new learning attempt.</summary>
    /// <param name="attempt">The attempt to persist.</param>
    /// <param name="cancellationToken">Cancels the write if requested.</param>
    /// <returns>The persisted attempt.</returns>
    Task<LearningAttempt> CreateAsync(LearningAttempt attempt, CancellationToken cancellationToken = default);

    /// <summary>Lists attempts belonging to one learner.</summary>
    /// <param name="learnerId">The authenticated learner's identifier.</param>
    /// <param name="cancellationToken">Cancels the query if requested.</param>
    /// <returns>The learner's attempts ordered from newest to oldest.</returns>
    Task<IReadOnlyList<LearningAttempt>> GetByLearnerAsync(Guid learnerId, CancellationToken cancellationToken = default);

    /// <summary>Finds an attempt only when it belongs to the specified learner.</summary>
    /// <param name="attemptId">The attempt identifier.</param>
    /// <param name="learnerId">The owning learner's identifier.</param>
    /// <param name="cancellationToken">Cancels the query if requested.</param>
    /// <returns>The owned attempt, or <see langword="null"/> when it does not exist.</returns>
    Task<LearningAttempt?> GetByIdForLearnerAsync(Guid attemptId, Guid learnerId, CancellationToken cancellationToken = default);

    /// <summary>Creates or updates a stage result for an owned attempt.</summary>
    /// <param name="attemptId">The attempt identifier.</param>
    /// <param name="learnerId">The owning learner's identifier.</param>
    /// <param name="result">The result to persist.</param>
    /// <param name="cancellationToken">Cancels the write if requested.</param>
    /// <returns>The updated attempt, or <see langword="null"/> when it is not owned by the learner.</returns>
    Task<LearningAttempt?> SaveStageResultAsync(Guid attemptId, Guid learnerId, AttemptStageResult result, CancellationToken cancellationToken = default);
}
