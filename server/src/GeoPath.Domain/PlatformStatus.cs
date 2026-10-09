namespace GeoPath.Domain;

/// <summary>
/// Describes the platform state returned by the health endpoint.
/// </summary>
public sealed class PlatformStatus
{
    /// <summary>Gets the platform's display name.</summary>
    public string ApplicationName { get; init; } = "GeoPath";

    /// <summary>Gets the current operational status.</summary>
    public string Status { get; init; } = "Operational";

    /// <summary>Gets the current application version.</summary>
    public string Version { get; init; } = "0.1.0";

    /// <summary>Gets the name of the active hosting environment.</summary>
    public string Environment { get; init; } = "Development";

    /// <summary>Gets the UTC time at which the status was generated.</summary>
    public DateTimeOffset TimestampUtc { get; init; } = DateTimeOffset.UtcNow;

    /// <summary>Gets the technologies used by the platform.</summary>
    public IReadOnlyList<string> TechStack { get; init; } = ["Angular", "C#/.NET", "PostgreSQL"];
}
