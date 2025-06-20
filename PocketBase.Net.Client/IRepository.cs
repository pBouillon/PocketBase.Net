using PocketBase.Net.Client.Entities;

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
    /// <param name="record">The payload containing the data used to create the new record.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The newly created record of type <typeparamref name="TRecord"/>.</returns>
    Task<TRecord> CreateRecord<TPayload>(TPayload record, CancellationToken cancellationToken);
}

/// <summary>
/// Base repository for managing records of type <typeparamref name="TRecord"/>.
/// </summary>
/// <param name="pocketBaseClient">The PocketBase client used to interact with the PocketBase API.</param>
/// <typeparam name="TRecord">The type of record, which must inherit from <see cref="RecordBase"/>.</typeparam>
public abstract class BaseRepository<TRecord>(IPocketBaseClient pocketBaseClient)
    : IRepository<TRecord>
    where TRecord : RecordBase
{
    /// <summary>
    /// Gets the collection id or name associated with this repository.
    /// </summary>
    public abstract string CollectionIdOrName { get; }

    // TODO - Add hooks (on create, on success, ...)

    /// <inheritdoc/>
    public Task<TRecord> CreateRecord<TPayload>(TPayload payload, CancellationToken cancellationToken = default)
        // TODO - Handle errors (collection non-existent, unauthenticated, ...)
        => pocketBaseClient.SendPost<TPayload, TRecord>(CollectionIdOrName, payload, cancellationToken);
}
