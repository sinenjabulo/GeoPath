namespace GeoPath.Domain;

public sealed class PlatformStatus
{
    public string ApplicationName { get; init; } = "GeoPath";
    public string Status { get; init; } = "Operational";
    public string Version { get; init; } = "0.1.0";
    public string Environment { get; init; } = "Development";
    public DateTimeOffset TimestampUtc { get; init; } = DateTimeOffset.UtcNow;
    public IReadOnlyList<string> TechStack { get; init; } = ["Angular", "C#/.NET", "PostgreSQL"];
}
