namespace GeoPath.Domain;

/// <summary>
/// Represents a GeoPath account and its authentication-related details.
/// </summary>
public sealed class User
{
    /// <summary>Gets or sets the user's unique identifier.</summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>Gets or sets the user's display name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Gets or sets the email address used to identify the user.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Gets or sets the one-way hash of the user's password.</summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>Gets or sets the user's platform role.</summary>
    public UserRole Role { get; set; } = UserRole.Visitor;

    /// <summary>Gets or sets when the account was created in UTC.</summary>
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    /// <summary>Gets or sets when the user last signed in, in UTC.</summary>
    public DateTimeOffset? LastLoginAtUtc { get; set; }
}
