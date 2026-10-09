using GeoPath.Domain;

namespace GeoPath.Application;

/// <summary>
/// Defines persistence operations for looking up and saving user accounts.
/// </summary>
public interface IUserRepository
{
    /// <summary>Finds a user by unique identifier.</summary>
    /// <param name="id">The user's identifier.</param>
    /// <param name="cancellationToken">Cancels the lookup if requested.</param>
    /// <returns>The matching user, or <see langword="null"/> when not found.</returns>
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>Finds a user by email address.</summary>
    /// <param name="email">The email address to look up.</param>
    /// <param name="cancellationToken">Cancels the lookup if requested.</param>
    /// <returns>The matching user, or <see langword="null"/> when not found.</returns>
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>Creates or updates a user in the repository.</summary>
    /// <param name="user">The user to persist.</param>
    /// <param name="cancellationToken">Cancels the save if requested.</param>
    /// <returns>The saved user.</returns>
    Task<User> SaveAsync(User user, CancellationToken cancellationToken = default);
}
