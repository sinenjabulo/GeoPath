using GeoPath.Domain;

namespace GeoPath.Application;

/// <summary>
/// Contains the details required to create a user account.
/// </summary>
/// <param name="Name">The user's display name.</param>
/// <param name="Email">The user's email address, used to sign in.</param>
/// <param name="Password">The user's plain-text password, which is hashed before storage.</param>
public sealed record RegisterRequest(
    string Name,
    string Email,
    string Password);

/// <summary>
/// Contains the credentials used to sign in to an account.
/// </summary>
/// <param name="Email">The email address associated with the account.</param>
/// <param name="Password">The account password to verify.</param>
public sealed record LoginRequest(string Email, string Password);

/// <summary>
/// Exposes a user's public identity details without credentials.
/// </summary>
/// <param name="Id">The user's unique identifier.</param>
/// <param name="Name">The user's display name.</param>
/// <param name="Email">The user's email address.</param>
/// <param name="Role">The user's assigned role.</param>
public sealed record UserSummary(
    Guid Id,
    string Name,
    string Email,
    UserRole Role);

/// <summary>
/// Contains the authentication response returned after registration or sign-in.
/// </summary>
/// <param name="User">The authenticated user's public details.</param>
/// <param name="Token">The access token for authenticated API requests.</param>
/// <param name="ExpiresAtUtc">The UTC time when the access token expires.</param>
public sealed record AuthResponse(
    UserSummary User,
    string Token,
    DateTimeOffset ExpiresAtUtc);

/// <summary>
/// Holds an authenticated domain user and the token issued for that user.
/// </summary>
/// <param name="User">The authenticated domain user.</param>
/// <param name="Token">The user's access token.</param>
/// <param name="ExpiresAtUtc">The UTC time when the access token expires.</param>
public sealed record AuthenticationResult(
    User User,
    string Token,
    DateTimeOffset ExpiresAtUtc);
