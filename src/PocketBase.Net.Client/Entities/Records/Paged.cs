using System.Text.Json.Serialization;

namespace PocketBase.Net.Client.Entities.Records;

/// <summary>
/// Represents a paginated response from PocketBase, containing a subset of records and metadata about the pagination.
/// </summary>
/// <typeparam name="TRecord">The type of records contained in the paginated response, which must be a <see cref="RecordBase"/>.</typeparam>
public record Paged<TRecord> where TRecord : RecordBase
{
    /// <summary>
    /// Gets the list of records on the current page.
    /// </summary>
    public List<TRecord> Items { get; init; } = [];

    /// <summary>
    /// Gets the offset (number of items to skip) for the current page, corresponding to the "page" field in the JSON response from PocketBase.
    /// </summary>
    [JsonPropertyName("page")]
    public int PageOffset { get; init; } = 0;

    /// <summary>
    /// Gets the number of items per page, corresponding to the "perPage" field in the JSON response.
    /// </summary>
    [JsonPropertyName("perPage")]
    public int ItemsPerPage { get; init; } = 0;

    /// <summary>
    /// Gets the total number of items across all pages.
    /// </summary>
    public int TotalItems { get; init; } = 0;

    /// <summary>
    /// Gets the total number of pages available.
    /// </summary>
    public int TotalPages { get; init; } = 0;
}
