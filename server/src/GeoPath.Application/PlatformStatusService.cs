using GeoPath.Domain;
using Microsoft.AspNetCore.Hosting;

namespace GeoPath.Application;

/// <summary>
/// Builds the API health status using the current hosting environment.
/// </summary>
public sealed class PlatformStatusService
{
    /// <summary>Provides the active hosting environment name.</summary>
    private readonly IWebHostEnvironment _environment;

    /// <summary>Creates the status service for the specified host environment.</summary>
    /// <param name="environment">The web host environment used in health responses.</param>
    public PlatformStatusService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    /// <summary>Creates the current platform health status.</summary>
    /// <returns>Status details including environment and UTC check time.</returns>
    public PlatformStatus GetStatus()
    {
        return new PlatformStatus
        {
            Environment = _environment.EnvironmentName,
            Status = "Operational",
            TimestampUtc = DateTimeOffset.UtcNow,
        };
    }
}
