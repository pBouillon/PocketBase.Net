namespace PocketBase.Net.Client.Entities;

/// <summary>
/// Represents the pagination options for querying data from PocketBase.
/// These options are used to control the pagination of results when making requests to the PocketBase API.
/// </summary>
public sealed record PaginationOptions
{
    /// <summary>
    /// Default pagination options.
    /// </summary>
    public static readonly PaginationOptions Default = new();

    /// <summary>
    /// Gets the page number to retrieve.
    /// </summary>
    /// <remarks>
    /// The default value is 1, which corresponds to the first page of results in PocketBase.
    /// </remarks>
    public int PageNumber { get; init; } = 1;

    /// <summary>
    /// Gets the number of items to retrieve per page.
    /// </summary>
    /// <remarks>
    /// The default value is 30, which is the default number of items per page in PocketBase.
    /// </remarks>
    public int ItemsPerPage { get; init; } = 30;

    /// <summary>
    /// Converts the pagination options into a query parameter string.
    /// </summary>
    /// <returns>
    /// A string that can be directly appended to a URL as query parameters.
    /// </returns>
    public string ToQueryParameters()
        => $"page={PageNumber}&perPage={ItemsPerPage}";
}
