using System.Security.Cryptography;
using System.Text;
using GeoPath.Domain;
using GeoPath.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace GeoPath.Api;

public sealed record JoinClassRequest(string? JoinCode, string? Code);

public static class GeoPathProgramSupport
{
    public static string NormalizeJoinCode(string joinCode)
    {
        return joinCode.Trim().Replace("-", string.Empty).Replace(" ", string.Empty).ToUpperInvariant();
    }

    public static string HashJoinCode(string joinCode)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(joinCode));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    public static async Task<bool> EnsureDemoClassroomAsync(
        GeoPathDbContext dbContext,
        CancellationToken cancellationToken = default)
    {
        var hasAnyClassroom = await dbContext.Classrooms.AnyAsync(cancellationToken);
        if (hasAnyClassroom)
        {
            return true;
        }

        var teacher = await dbContext.Users
            .FirstOrDefaultAsync(user => user.Role == UserRole.Teacher, cancellationToken);
        if (teacher is null)
        {
            return false;
        }

        dbContext.Classrooms.Add(new Classroom
        {
            TeacherId = teacher.Id,
            Name = "Grade 10 Geometry",
            JoinCodeHash = HashJoinCode(NormalizeJoinCode("GEO123")),
            CreatedAtUtc = DateTimeOffset.UtcNow,
        });

        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
