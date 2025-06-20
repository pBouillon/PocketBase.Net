using PocketBase.Net.Client.Records.Traits;

namespace PocketBase.Net.Client.Records;

/// <summary>
/// Abstract base record that provides a foundation for PocketBase's records.
/// </summary>
public abstract record RecordBase
    : IIdentifiable, IAuditable, ICollectionned
{
    /// <inheritdoc/>
    public required string Id { get; set; }

    /// <inheritdoc/>
    public DateTime Created { get; set; }

    /// <inheritdoc/>
    public DateTime Updated { get; set; }

    /// <inheritdoc/>
    public required string CollectionId { get; set; }

    /// <inheritdoc/>
    public required string CollectionName { get; set; }
}

