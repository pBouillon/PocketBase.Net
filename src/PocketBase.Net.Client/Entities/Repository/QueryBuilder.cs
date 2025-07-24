using PocketBase.Net.Client.Entities.Filter;
using PocketBase.Net.Client.Entities.Records;

namespace PocketBase.Net.Client.Entities.Repository;

/// <summary>
/// A builder for constructing <see cref="Query{TRecord}"/> objects with a fluent interface.
/// </summary>
/// <typeparam name="TRecord">The type of record the query is for.</typeparam>
public sealed class QueryBuilder<TRecord>(IRepository<TRecord> Repository)
    where TRecord : RecordBase
{
    /// <summary>
    /// Inner instance of the query to generate.
    /// </summary>
    private Query<TRecord> _query = new();

    /// <summary>
    /// Returns the constructed <see cref="Query{TRecord}"/> object.
    /// </summary>
    /// <returns>The constructed query.</returns>
    public Query<TRecord> Build() => _query;

    /// <summary>
    /// Sets the filter condition for the query.
    /// </summary>
    /// <param name="filter">The filter condition to apply.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public QueryBuilder<TRecord> WithFilter(string filter)
    {
        _query = _query with { Filter = filter };
        return this;
    }

    /// <summary>
    /// Sets the pagination options for the query.
    /// </summary>
    /// <param name="paginationOptions">The pagination options to apply.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public QueryBuilder<TRecord> WithPagination(PaginationOptions paginationOptions)
    {
        _query = _query with { PaginationOptions = paginationOptions };
        return this;
    }

    /// <summary>
    /// Defines how the results should be sorted for the query.
    /// </summary>
    /// <param name="sorting">The sorting to apply.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public QueryBuilder<TRecord> WithSorting(string sorting)
    {
        _query = _query with { Sorting = sorting };
        return this;
    }

    /// <summary>
    /// Executes the constructed query on the repository.
    /// </summary>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation, containing the paged results.</returns>
    public Task<Paged<TRecord>> ExecuteAsync(CancellationToken cancellationToken = default)
        => Repository.GetRecords(_query, cancellationToken);
}
