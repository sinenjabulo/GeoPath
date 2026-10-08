using GeoPath.Domain;
using Microsoft.AspNetCore.Hosting;

namespace GeoPath.Application;

public sealed class PlatformStatusService
{
    private readonly IWebHostEnvironment _environment;

    public PlatformStatusService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

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
