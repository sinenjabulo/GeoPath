using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GeoPath.Application;
using GeoPath.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GeoPath.Infrastructure;

/// <summary>
/// Creates signed JWT access tokens containing a user's identity and role claims.
/// </summary>
public sealed class JwtTokenService : IAuthenticationTokenService
{
    /// <summary>Provides JWT issuer, audience, and signing-key configuration.</summary>
    private readonly IConfiguration _configuration;

    /// <summary>Creates the token service with application configuration.</summary>
    /// <param name="configuration">Configuration containing JWT settings.</param>
    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>Creates a signed access token for a user with a six-hour lifetime.</summary>
    /// <param name="user">The user identity and role to include in the token.</param>
    /// <returns>The serialized JWT access token.</returns>
    public string CreateToken(User user)
    {
        var issuer = _configuration["Authentication:JwtIssuer"] ?? "GeoPath";
        var audience = _configuration["Authentication:JwtAudience"] ?? "GeoPath";
        var secret = _configuration["Authentication:JwtSecret"] ?? "GeoPath-Development-Secret-Replace-In-Production";

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(6),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
