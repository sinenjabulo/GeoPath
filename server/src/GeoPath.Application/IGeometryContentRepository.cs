using GeoPath.Domain;

namespace GeoPath.Application;

/// <summary>Defines read operations for published geometry learning content.</summary>
public interface IGeometryContentRepository
{
    /// <summary>Gets all published examples with their topic summaries.</summary>
    /// <param name="cancellationToken">Cancels the query if requested.</param>
    /// <returns>Published examples ordered by title.</returns>
    Task<IReadOnlyList<GeometryExample>> GetPublishedExamplesAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets one published example and its theorem and definition references.</summary>
    /// <param name="id">The example identifier.</param>
    /// <param name="cancellationToken">Cancels the query if requested.</param>
    /// <returns>The matching published example, or <see langword="null"/> when it does not exist.</returns>
    Task<GeometryExample?> GetPublishedExampleAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>Gets all published theorems ordered by name.</summary>
    /// <param name="cancellationToken">Cancels the query if requested.</param>
    /// <returns>The published theorem list.</returns>
    Task<IReadOnlyList<Theorem>> GetPublishedTheoremsAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets all published definitions ordered by name.</summary>
    /// <param name="cancellationToken">Cancels the query if requested.</param>
    /// <returns>The published definition list.</returns>
    Task<IReadOnlyList<GeometryDefinition>> GetPublishedDefinitionsAsync(CancellationToken cancellationToken = default);
}
