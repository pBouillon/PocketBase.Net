using PocketBase.Net.Client.Configuration;
using PocketBase.Net.Client.Entities;
using PocketBase.Net.Client.Entities.Records;
using PocketBase.Net.Client.Entities.Users;

namespace PocketBase.Net.Client;

/// <summary>
/// Specify the contract of an entity to interact with PocketBase on a lower-level
/// </summary>
public interface IPocketBaseClient
{
    /// <summary>
    /// Authenticates the client with the PocketBase server.
    /// </summary>
    /// <param name="cancellation">A cancellation token to cancel the operation.</param>
    /// <returns>The authenticated <see cref="PocketBaseUser"/>.</returns>
    Task<PocketBaseUser> Authenticate(CancellationToken cancellation = default);

    /// <summary>
    /// Sends a POST request to the web API to a specified collection to create a new record.
    /// </summary>
    /// <remarks>
    /// <see href="https://pocketbase.io/docs/api-records/#create-record"/>
    /// </remarks>
    /// <typeparam name="TPayload">The type of the payload to send in the body of the POST request.</typeparam>
    /// <typeparam name="TRecord">The type of the expected response, which should be a subclass of <see cref="RecordBase"/>.</typeparam>
    /// <param name="collectionIdOrName">The id or name of the collection where the record will be created.</param>
    /// <param name="payload">The payload to be serialized as the body of the POST request.</param>
    /// <param name="cancellation">A cancellation token to cancel the operation.</param>
    /// <returns>The newly created record of type <typeparamref name="TRecord"/>.</returns>
    Task<TRecord> CreateRecord<TPayload, TRecord>(
        string collectionIdOrName,
        TPayload payload,
        CancellationToken cancellation = default
    ) where TRecord : RecordBase;

    /// <summary>
    /// Sends a DELETE request to the web API to a specified collection to delete a record by its id.
    /// </summary>
    /// <remarks>
    /// <see href="https://pocketbase.io/docs/api-records/#delete-record"/>
    /// </remarks>
    /// <param name="collectionIdOrName">The id or name of the collection where the record will be deleted.</param>
    /// <param name="recordId">The id of the record to delete.</param>
    /// <param name="cancellation">A cancellation token to cancel the operation.</param>
    Task DeleteRecord(
        string collectionIdOrName,
        string recordId,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Sends a GET request to the web API to a specified collection to get a record by its id.
    /// </summary>
    /// <remarks>
    /// <see href="https://pocketbase.io/docs/api-records/#view-record"/>
    /// </remarks>
    /// <param name="collectionIdOrName">The id or name of the collection from which the record will be retrieved.</param>
    /// <param name="recordId">The id of the record to retrieve.</param>
    /// <param name="cancellation">A cancellation token to cancel the operation.</param>
    /// <returns>The retrieved record.</returns>
    Task<TRecord> GetRecord<TRecord>(
        string collectionIdOrName,
        string recordId,
        CancellationToken cancellation = default
     ) where TRecord : RecordBase;

    /// <summary>
    /// Sends a GET request to the web API to a specified collection to get all records of a collection.
    /// </summary>
    /// <remarks>
    /// <see href="https://pocketbase.io/docs/api-records/#listsearch-records"/>
    /// </remarks>
    /// <param name="collectionIdOrName">The id or name of the collection from which the record will be retrieved.</param>
    /// <param name="filter">An optional filter to apply on the search result.</param>
    /// <param name="paginationOptions">An optional pagination on the search result.</param>
    /// <param name="cancellation">A cancellation token to cancel the operation.</param>
    /// <returns>The retrieved records.</returns>
    Task<Paged<TRecord>> GetRecords<TRecord>(
        string collectionIdOrName,
        string? filter = null,
        PaginationOptions? paginationOptions = null,
        CancellationToken cancellation = default
     ) where TRecord : RecordBase;

    /// <summary>
    /// Sends a PATCH request to the web API to a specified collection to update a given record.
    /// </summary>
    /// <remarks>
    /// <see href="https://pocketbase.io/docs/api-records/#update-record"/>
    /// </remarks>
    /// <typeparam name="TRecord">The type of the expected response, which should be a subclass of <see cref="RecordBase"/>.</typeparam>
    /// <param name="collectionIdOrName">The id or name of the collection where the record will be created.</param>
    /// <param name="recordId">The id of the record to update.</param>
    /// <param name="payload">A <see cref="IDictionary"/> of properties to update.</param>
    /// <param name="cancellation">A cancellation token to cancel the operation.</param>
    /// <returns>The updated record of type <typeparamref name="TRecord"/>.</returns>
    Task<TRecord> UpdateRecord<TRecord>(
        string collectionIdOrName,
        string recordId,
        IDictionary<string, object?> payload,
        CancellationToken cancellation = default
    ) where TRecord : RecordBase;
}

/// <summary>
/// Provides a lower-level interaction with PocketBase through <see cref="PocketBaseHttpClientWrapper"/>.
/// </summary>
/// <param name="configuration">The configuration for the PocketBase client.</param>
/// <param name="httpClientWrapper">The HTTP client wrapper used to interact with the PocketBase web API.</param>
public class PocketBaseClient(
    PocketBaseClientConfiguration configuration,
    PocketBaseHttpClientWrapper httpClientWrapper
) : IPocketBaseClient
{
    /// <inheritdoc/>
    public Task<PocketBaseUser> Authenticate(CancellationToken cancellation = default)
        => httpClientWrapper.AuthenticateUsing(configuration.ClientCredentials, cancellation);

    /// <inheritdoc/>
    public Task<TRecord> CreateRecord<TPayload, TRecord>(
        string collectionIdOrName,
        TPayload payload,
        CancellationToken cancellationToken = default
    ) where TRecord : RecordBase
        => httpClientWrapper.SendPost<TPayload, TRecord>(collectionIdOrName, payload, cancellationToken);

    /// <inheritdoc/>
    public Task DeleteRecord(
        string collectionIdOrName,
        string recordId,
        CancellationToken cancellationToken = default
    )
        => httpClientWrapper.SendDelete(collectionIdOrName, recordId, cancellationToken);

    /// <inheritdoc/>
    public Task<TRecord> GetRecord<TRecord>(
        string collectionIdOrName,
        string recordId,
        CancellationToken cancellationToken = default
    ) where TRecord : RecordBase
        => httpClientWrapper.SendGetById<TRecord>(collectionIdOrName, recordId, cancellationToken);

    /// <inheritdoc/>
    public Task<Paged<TRecord>> GetRecords<TRecord>(
        string collectionIdOrName,
        string? filter = null,
        PaginationOptions? paginationOptions = null,
        CancellationToken cancellationToken = default
    ) where TRecord : RecordBase
        => httpClientWrapper.SendGet<TRecord>(
            collectionIdOrName,
            filter,
            paginationOptions,
            cancellationToken);

    /// <inheritdoc/>
    public Task<TRecord> UpdateRecord<TRecord>(
        string collectionIdOrName,
        string recordId,
        IDictionary<string, object?> payload,
        CancellationToken cancellationToken = default
    ) where TRecord : RecordBase
        => httpClientWrapper.SendPatch<TRecord>(collectionIdOrName, recordId, payload, cancellationToken);
}
