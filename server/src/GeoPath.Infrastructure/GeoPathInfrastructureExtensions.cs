using GeoPath.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GeoPath.Infrastructure;

/// <summary>Registers PostgreSQL persistence and repository implementations.</summary>
public static class GeoPathInfrastructureExtensions
{
    /// <summary>Adds the configured PostgreSQL context and GeoPath repositories.</summary>
    /// <param name="services">The dependency injection container.</param>
    /// <param name="configuration">Application configuration containing a database connection string.</param>
    /// <returns>The service collection for further configuration.</returns>
    /// <exception cref="InvalidOperationException">No PostgreSQL connection string is configured.</exception>
    public static IServiceCollection AddGeoPathInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? Environment.GetEnvironmentVariable("GEOPATH_DB_CONNECTION");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Configure ConnectionStrings:DefaultConnection or the GEOPATH_DB_CONNECTION environment variable.");
        }

        services.AddDbContext<GeoPathDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<PostgresGeoPathRepository>();
        services.AddScoped<IUserRepository>(provider => provider.GetRequiredService<PostgresGeoPathRepository>());
        services.AddScoped<IGeometryContentRepository>(provider => provider.GetRequiredService<PostgresGeoPathRepository>());
        services.AddScoped<ILearningAttemptRepository>(provider => provider.GetRequiredService<PostgresGeoPathRepository>());

        return services;
    }
}
