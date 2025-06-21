using PocketBase.Net.Client.Configuration;
using PocketBase.Net.Client.Entities.Records;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

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
    public string CollectionName { get; init; } = GetDefaultCollectionName();

    // TODO - Naming strategy / Naming override on demand
    private static string GetDefaultCollectionName()
    {
        var recordName = typeof(TRecord).Name;

        var withTrimmedSuffix = Regex.Replace(recordName, @"Record$", string.Empty);

        var camelCaseName = char.ToLowerInvariant(withTrimmedSuffix[0]) + withTrimmedSuffix[1..];

        return camelCaseName.EndsWith('s') ? camelCaseName : camelCaseName + 's';
    }

    /// <inheritdoc/>
    public Task<TRecord> CreateRecordFrom<TPayload>(TPayload payload, CancellationToken cancellationToken = default)
    {
        // TODO - Check against validator

        return pocketBaseClient.SendPost<TPayload, TRecord>(CollectionName, payload, cancellationToken);
    }

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

        return pocketBaseClient.SendPatch<TRecord>(CollectionName, recordId, payload, cancellationToken);
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
