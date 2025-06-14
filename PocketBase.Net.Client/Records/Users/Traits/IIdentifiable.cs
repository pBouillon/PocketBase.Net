namespace PocketBase.Net.Client.Records.Users.Traits;

/// <summary>
/// Trait to represent an entity that can be identified by an identifier.
/// </summary>
public interface IIdentifiable
{
    /// <summary>
    /// The entitie's identifier.
    /// </summary>
    public string Id { get; set; }
}
