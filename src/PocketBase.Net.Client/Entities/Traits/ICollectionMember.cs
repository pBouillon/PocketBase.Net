namespace PocketBase.Net.Client.Entities.Traits;

/// <summary>
/// Interface for entities that are members of a PocketBase's collection, providing access to collection metadata.
/// </summary>
public interface ICollectionMember
{
    /// <summary>
    /// The collection identifier.
    /// </summary>
    string CollectionId { get; }

    /// <summary>
    /// The collection name.
    /// </summary>
    string CollectionName { get; }
}
