using PocketBase.Net.Client.Entities.Traits;

namespace PocketBase.Net.Client.Entities.Records;

/// <summary>
/// Abstract base record that provides a foundation for PocketBase's records.
/// </summary>
public abstract record RecordBase
    : IIdentifiable, IAuditable, ICollectionMember
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
