using PocketBase.Net.Client.Configuration;
using PocketBase.Net.Client.Entities.Records;
using System.Collections.Immutable;

namespace PocketBase.Net.Client;

/// <summary>
/// Repository contract to manage <typeparamref name="TRecord"/> records.
/// </summary>
/// <typeparam name="TRecord">The type of record, which must inherit from <see cref="RecordBase"/>.</typeparam>
public interface IRepository<TRecord>
    where TRecord : RecordBase
{
    /// <summary>
    /// Creates a new record.
    /// </summary>
    /// <typeparam name="TPayload">The type of the payload to create the record.</typeparam>
    /// <param name="payload">The payload containing the data used to create the new record.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The newly created record of type <typeparamref name="TRecord"/>.</returns>
    Task<TRecord> CreateRecordFrom<TPayload>(TPayload payload, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new record. Upon error, invoke <paramref name="onError"/> instead of throwing and returns <c>null</c>.
    /// </summary>
    /// <typeparam name="TPayload">The type of the payload to create the record.</typeparam>
    /// <param name="payload">The payload containing the data used to create the new record.</param>
    /// <param name="onError">An action to execute upon error.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The newly created record of type <typeparamref name="TRecord"/>.</returns>
    Task<TRecord?> CreateRecordFrom<TPayload>(
        TPayload payload,
        Action<TPayload, Exception> onError,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete an existing record.
    /// </summary>
    /// <param name="recordId">The ID of the record to delete.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    Task DeleteRecord(
        string recordId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete an existing record. Upon error, invoke <paramref name="onError"/> instead of throwing and returns <c>null</c>.
    /// </summary>
    /// <param name="recordId">The ID of the record to delete.</param>
    /// <param name="onError">An action to execute upon error.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    Task DeleteRecord(
        string recordId,
        Action<Exception> onError,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieve a record.
    /// </summary>
    /// <param name="recordId">The ID of the record to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The retrieved record of type <typeparamref name="TRecord"/>.</returns>
    Task<TRecord> GetRecord(
        string recordId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieve an existing record. Upon error, invoke <paramref name="onError"/>
    /// instead of throwing and returns <c>null</c>.
    /// </summary>
    /// <param name="recordId">The ID of the record to retrieve.</param>
    /// <param name="onError">An action to execute upon error.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The retrieved record of type <typeparamref name="TRecord"/>.</returns>
    Task<TRecord?> GetRecord(
        string recordId,
        Action<string, Exception> onError,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieve all records.
    /// </summary>
    /// <param name="filter">An optional filter to apply on the search result.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The retrieved records of type <typeparamref name="TRecord"/>.</returns>
    Task<Paged<TRecord>> GetRecords(string? filter = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieve all records. Upon error, invoke <paramref name="onError"/>
    /// instead of throwing and returns <c>null</c>.
    /// </summary>
    /// <param name="onError">An action to execute upon error.</param>
    /// <param name="filter">An optional filter to apply on the search result.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The retrieved records of type <typeparamref name="TRecord"/>.</returns>
    Task<Paged<TRecord>?> GetRecords(
        Action<Exception> onError,
        string? filter = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing record with the provided payload.
    /// </summary>
    /// <param name="recordId">The ID of the record to update.</param>
    /// <param name="payload">The payload containing the data used to update the record.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The updated record of type <typeparamref name="TRecord"/>.</returns>
    Task<TRecord> UpdateRecord(
        string recordId,
        object payload,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing record with the provided dictionary payload.
    /// </summary>
    /// <param name="recordId">The ID of the record to update.</param>
    /// <param name="payload">The dictionary containing the data used to update the record.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The updated record of type <typeparamref name="TRecord"/>.</returns>
    Task<TRecord> UpdateRecord(
        string recordId,
        IDictionary<string, object?> payload,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing record with the provided payload. Upon error, invoke <paramref name="onError"/> instead of throwing and returns <c>null</c>.
    /// </summary>
    /// <param name="recordId">The ID of the record to update.</param>
    /// <param name="payload">The payload containing the data used to update the record.</param>
    /// <param name="onError">An action to execute upon error.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The updated record of type <typeparamref name="TRecord"/>.</returns>
    Task<TRecord?> UpdateRecord(
        string recordId,
        object payload,
        Action<object, Exception> onError,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing record with the provided dictionary payload. Upon error, invoke <paramref name="onError"/> instead of throwing and returns <c>null</c>.
    /// </summary>
    /// <param name="recordId">The ID of the record to update.</param>
    /// <param name="payload">The dictionary containing the data used to update the record.</param>
    /// <param name="onError">An action to execute upon error.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The updated record of type <typeparamref name="TRecord"/>.</returns>
    Task<TRecord?> UpdateRecord(
        string recordId,
        IDictionary<string, object?> payload,
        Action<IDictionary<string, object?>, Exception> onError,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Base repository for managing records of type <typeparamref name="TRecord"/>.
/// </summary>
/// <param name="pocketBaseClient">The PocketBase client used to interact with the PocketBase API.</param>
/// <typeparam name="TRecord">The type of record, which must inherit from <see cref="RecordBase"/>.</typeparam>
public class Repository<TRecord>(
    IPocketBaseClient pocketBaseClient,
    PocketBaseClientConfiguration configuration,
    RecordValidator<TRecord> validator
) : IRepository<TRecord>
    where TRecord : RecordBase
{
    /// <summary>
    /// Gets the collection id or name associated with this repository.
    /// </summary>
    public string CollectionName { get; init; } = configuration.CollectionNamingPipeline.GetCollectionNameOf<TRecord>();

    /// <inheritdoc/>
    public Task<TRecord> CreateRecordFrom<TPayload>(TPayload payload, CancellationToken cancellationToken = default)
        => pocketBaseClient.CreateRecord<TPayload, TRecord>(CollectionName, payload, cancellationToken);

    /// <inheritdoc/>
    public async Task<TRecord?> CreateRecordFrom<TPayload>(
        TPayload payload,
        Action<TPayload, Exception> onError,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await CreateRecordFrom(payload, cancellationToken);
        }
        catch (Exception exception)
        {
            onError.Invoke(payload, exception);
            return null;
        }
    }

    /// <inheritdoc/>
    public Task DeleteRecord(string recordId, CancellationToken cancellationToken = default)
        => pocketBaseClient.DeleteRecord(CollectionName, recordId, cancellationToken);

    /// <inheritdoc/>
    public async Task DeleteRecord(string recordId, Action<Exception> onError, CancellationToken cancellationToken = default)
    {
        try
        {
            await DeleteRecord(recordId, cancellationToken);
        }
        catch (Exception exception)
        {
            onError.Invoke(exception);
        }
    }

    /// <inheritdoc/>
    public Task<TRecord> GetRecord(string recordId, CancellationToken cancellationToken = default)
        => pocketBaseClient.GetRecord<TRecord>(CollectionName, recordId, cancellationToken);

    /// <inheritdoc/>
    public async Task<TRecord?> GetRecord(string recordId, Action<string, Exception> onError, CancellationToken cancellationToken = default)
    {
        try
        {
            return await GetRecord(recordId, cancellationToken);
        }
        catch (Exception exception)
        {
            onError.Invoke(recordId, exception);
            return null;
        }
    }

    /// <inheritdoc/>
    public Task<Paged<TRecord>> GetRecords(
        string? filter = null,
        CancellationToken cancellationToken = default)
        => pocketBaseClient.GetRecords<TRecord>(CollectionName, filter, cancellationToken);

    /// <inheritdoc/>
    public async Task<Paged<TRecord>?> GetRecords(
        Action<Exception> onError,
        string? filter = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await GetRecords(filter, cancellationToken);
        }
        catch (Exception ex)
        {
            onError.Invoke(ex);
            return null;
        }
    }

    /// <inheritdoc/>
    public Task<TRecord> UpdateRecord(
        string recordId,
        IDictionary<string, object?> payload,
        CancellationToken cancellationToken = default)
    {
        var shouldCheckUnknownProperties = !configuration.RecordOperationBehavior
            .HasFlag(RecordOperationBehavior.IgnoreUnknownProperties);

        if (shouldCheckUnknownProperties)
        {
            validator.ThrowOnUnknownPropertiesIn(payload);
        }

        var shouldCheckPropertyTypeMismatches = !configuration.RecordOperationBehavior
            .HasFlag(RecordOperationBehavior.IgnorePropertyTypeMismatches);

        if (shouldCheckPropertyTypeMismatches)
        {
            validator.ThrowOnMismatchedPropertyTypesIn(payload);
        }

        return pocketBaseClient.UpdateRecord<TRecord>(CollectionName, recordId, payload, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<TRecord> UpdateRecord(
        string recordId,
        object payload,
        CancellationToken cancellationToken = default)
    {
        var mappedPayload = payload
                .GetType()
                .GetProperties()
                .ToImmutableDictionary(
                    property => property.Name,
                    property => property.GetValue(payload));

        return UpdateRecord(recordId, mappedPayload, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<TRecord?> UpdateRecord(
        string recordId,
        IDictionary<string, object?> payload,
        Action<IDictionary<string, object?>, Exception> onError,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await UpdateRecord(recordId, payload, cancellationToken);
        }
        catch (Exception exception)
        {
            onError(payload, exception);
            return null;
        }
    }

    /// <inheritdoc/>
    public async Task<TRecord?> UpdateRecord(
        string recordId,
        object payload,
        Action<object, Exception> onError,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await UpdateRecord(recordId, payload, cancellationToken);
        }
        catch (Exception exception)
        {
            onError(payload, exception);
            return null;
        }
    }
}
