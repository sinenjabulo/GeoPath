using GeoPath.Domain;

namespace GeoPath.Application;

/// <summary>
/// Defines the service used to issue authentication tokens for users.
/// </summary>
public interface IAuthenticationTokenService
{
    /// <summary>Creates a signed access token representing the specified user.</summary>
    /// <param name="user">The user whose identity and role are placed in the token.</param>
    /// <returns>The serialized authentication token.</returns>
    string CreateToken(User user);
}
