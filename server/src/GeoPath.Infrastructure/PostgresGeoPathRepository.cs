using GeoPath.Application;
using GeoPath.Domain;
using Microsoft.EntityFrameworkCore;

namespace GeoPath.Infrastructure;

/// <summary>Persists accounts, public content, and learner attempts in PostgreSQL.</summary>
public sealed class PostgresGeoPathRepository :
    IUserRepository,
    IGeometryContentRepository,
    ILearningAttemptRepository
{
    /// <summary>Provides PostgreSQL queries and change tracking for repository operations.</summary>
    private readonly GeoPathDbContext _dbContext;

    /// <summary>Creates the repositories using the shared GeoPath database context.</summary>
    /// <param name="dbContext">The scoped PostgreSQL database context.</param>
    public PostgresGeoPathRepository(GeoPathDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>Finds a user account by its unique identifier.</summary>
    /// <param name="id">The account identifier.</param>
    /// <param name="cancellationToken">Cancels the database query if requested.</param>
    /// <returns>The matching user, or <see langword="null"/> when not found.</returns>
    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Users.AsNoTracking()
            .SingleOrDefaultAsync(user => user.Id == id, cancellationToken);
    }

    /// <summary>Finds a user account by normalized email address.</summary>
    /// <param name="email">The normalized email address to look up.</param>
    /// <param name="cancellationToken">Cancels the database query if requested.</param>
    /// <returns>The matching user, or <see langword="null"/> when not found.</returns>
    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();
        return _dbContext.Users.AsNoTracking()
            .SingleOrDefaultAsync(user => user.Email == normalizedEmail, cancellationToken);
    }

    /// <summary>Creates or updates a user account.</summary>
    /// <param name="user">The account to persist.</param>
    /// <param name="cancellationToken">Cancels the database write if requested.</param>
    /// <returns>The persisted user.</returns>
    public async Task<User> SaveAsync(User user, CancellationToken cancellationToken = default)
    {
        user.Email = user.Email.Trim().ToLowerInvariant();
        var trackedUser = await _dbContext.Users.FindAsync([user.Id], cancellationToken);
        if (trackedUser is null)
        {
            _dbContext.Users.Add(user);
        }
        else
        {
            _dbContext.Entry(trackedUser).CurrentValues.SetValues(user);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        return user;
    }

    /// <summary>Gets published examples and their topic summaries.</summary>
    /// <param name="cancellationToken">Cancels the database query if requested.</param>
    /// <returns>Published examples ordered by title.</returns>
    public async Task<IReadOnlyList<GeometryExample>> GetPublishedExamplesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Examples.AsNoTracking()
            .Where(example => example.IsPublished)
            .Include(example => example.Topic)
            .Include(example => example.Theorems)
                .ThenInclude(link => link.Theorem)
            .Include(example => example.Definitions)
                .ThenInclude(link => link.Definition)
            .OrderBy(example => example.Title)
            .ToListAsync(cancellationToken);
    }

    /// <summary>Gets a published example and its related theorems and definitions.</summary>
    /// <param name="id">The example identifier.</param>
    /// <param name="cancellationToken">Cancels the database query if requested.</param>
    /// <returns>The published example, or <see langword="null"/> when not found.</returns>
    public Task<GeometryExample?> GetPublishedExampleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Examples.AsNoTracking()
            .Where(example => example.Id == id && example.IsPublished)
            .Include(example => example.Topic)
            .Include(example => example.Theorems)
                .ThenInclude(link => link.Theorem)
            .Include(example => example.Definitions)
                .ThenInclude(link => link.Definition)
            .SingleOrDefaultAsync(cancellationToken);
    }

    /// <summary>Gets all published theorems ordered by name.</summary>
    /// <param name="cancellationToken">Cancels the database query if requested.</param>
    /// <returns>The published theorem list.</returns>
    public async Task<IReadOnlyList<Theorem>> GetPublishedTheoremsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Theorems.AsNoTracking()
            .Where(theorem => theorem.IsPublished)
            .OrderBy(theorem => theorem.Name)
            .ToListAsync(cancellationToken);
    }

    /// <summary>Gets all published definitions ordered by name.</summary>
    /// <param name="cancellationToken">Cancels the database query if requested.</param>
    /// <returns>The published definition list.</returns>
    public async Task<IReadOnlyList<GeometryDefinition>> GetPublishedDefinitionsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Definitions.AsNoTracking()
            .Where(definition => definition.IsPublished)
            .OrderBy(definition => definition.Name)
            .ToListAsync(cancellationToken);
    }

    /// <summary>Persists a new learner attempt.</summary>
    /// <param name="attempt">The attempt to create.</param>
    /// <param name="cancellationToken">Cancels the database write if requested.</param>
    /// <returns>The persisted attempt.</returns>
    public async Task<LearningAttempt> CreateAsync(LearningAttempt attempt, CancellationToken cancellationToken = default)
    {
        _dbContext.Attempts.Add(attempt);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return attempt;
    }

    /// <summary>Lists attempts for one learner with their stage results.</summary>
    /// <param name="learnerId">The owning learner's identifier.</param>
    /// <param name="cancellationToken">Cancels the database query if requested.</param>
    /// <returns>The learner's attempts ordered from newest to oldest.</returns>
    public async Task<IReadOnlyList<LearningAttempt>> GetByLearnerAsync(
        Guid learnerId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Attempts.AsNoTracking()
            .Where(attempt => attempt.LearnerId == learnerId)
            .Include(attempt => attempt.StageResults)
            .OrderByDescending(attempt => attempt.StartedAtUtc)
            .ToListAsync(cancellationToken);
    }

    /// <summary>Gets an attempt only when it is owned by the specified learner.</summary>
    /// <param name="attemptId">The attempt identifier.</param>
    /// <param name="learnerId">The owning learner's identifier.</param>
    /// <param name="cancellationToken">Cancels the database query if requested.</param>
    /// <returns>The owned attempt, or <see langword="null"/> when not found.</returns>
    public Task<LearningAttempt?> GetByIdForLearnerAsync(
        Guid attemptId,
        Guid learnerId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Attempts.AsNoTracking()
            .Where(attempt => attempt.Id == attemptId && attempt.LearnerId == learnerId)
            .Include(attempt => attempt.StageResults)
            .SingleOrDefaultAsync(cancellationToken);
    }

    /// <summary>Creates or updates one stage result on an attempt owned by the learner.</summary>
    /// <param name="attemptId">The attempt identifier.</param>
    /// <param name="learnerId">The owning learner's identifier.</param>
    /// <param name="result">The stage result to persist.</param>
    /// <param name="cancellationToken">Cancels the database write if requested.</param>
    /// <returns>The updated attempt, or <see langword="null"/> when not owned by the learner.</returns>
    public async Task<LearningAttempt?> SaveStageResultAsync(
        Guid attemptId,
        Guid learnerId,
        AttemptStageResult result,
        CancellationToken cancellationToken = default)
    {
        var attempt = await _dbContext.Attempts
            .Where(candidate => candidate.Id == attemptId && candidate.LearnerId == learnerId)
            .Include(candidate => candidate.StageResults)
            .SingleOrDefaultAsync(cancellationToken);
        if (attempt is null)
        {
            return null;
        }

        var existingResult = attempt.StageResults.SingleOrDefault(candidate => candidate.Stage == result.Stage);
        if (existingResult is null)
        {
            result.AttemptId = attempt.Id;
            _dbContext.AttemptStageResults.Add(result);
        }
        else
        {
            existingResult.Outcome = result.Outcome;
            existingResult.ResponseJson = result.ResponseJson;
            existingResult.RecordedAtUtc = DateTimeOffset.UtcNow;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        return attempt;
    }
}
