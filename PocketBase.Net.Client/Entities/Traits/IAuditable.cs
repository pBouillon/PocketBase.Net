namespace PocketBase.Net.Client.Entities.Traits;

/// <summary>
/// Trait representing an entity whose modification operations can be tracked.
/// </summary>
public interface IAuditable
{
    /// <summary>
    /// The creation date of this entity.
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    /// The modification date of this entity.
    /// </summary>
    public DateTime Updated { get; set; }
}
