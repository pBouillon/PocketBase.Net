using PocketBase.Net.Client.Entities.Records;

namespace PocketBase.Net.Client.Entities.Repository;

/// <summary>
/// A query specification with filter and pagination options for retrieving records of type <typeparamref name="TRecord"/>.
/// </summary>
/// <typeparam name="TRecord">The type of record to query.</typeparam>
public sealed record Query<TRecord>
    where TRecord : RecordBase
{
    /// <summary>
    /// Filter condition for the query. Defaults to an empty string.
    /// </summary>
    public string Filter { get; init; } = string.Empty;

    /// <summary>
    /// Pagination options for the query. Defaults to a <see cref="PaginationOptions.Default"/>.
    /// </summary>
    public PaginationOptions PaginationOptions { get; init; } = PaginationOptions.Default;

    /// <summary>
    /// Sorting to apply on the search results. Defaults to an empty string.
    /// </summary>
    public string Sorting { get; init; } = string.Empty;
}
