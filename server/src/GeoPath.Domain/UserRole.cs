namespace GeoPath.Domain;

/// <summary>
/// Defines the roles used to identify GeoPath account access levels.
/// </summary>
public enum UserRole
{
    /// <summary>A non-authenticated user with access to public content.</summary>
    Visitor = 0,

    /// <summary>A student who uses learning and practice features.</summary>
    Learner = 1,

    /// <summary>An educator who manages classes and learner activity.</summary>
    Teacher = 2,

    /// <summary>A content creator who maintains educational material.</summary>
    Author = 3,

    /// <summary>A platform administrator with elevated access.</summary>
    Admin = 4,
}

/// <summary>Represents a role available for assignment to a GeoPath user.</summary>
public sealed class RoleDefinition
{
    /// <summary>Gets or sets the stable role identifier.</summary>
    public UserRole Id { get; set; }

    /// <summary>Gets or sets the role's display name.</summary>
    public string Name { get; set; } = string.Empty;
}
