namespace PocketBase.Net.Client.Entities.Traits;

/// <summary>
/// Trait to represent an entity that can be identified by an identifier.
/// </summary>
public interface IIdentifiable
{
    /// <summary>
    /// The entity's identifier.
    /// </summary>
    public string Id { get; set; }
}
