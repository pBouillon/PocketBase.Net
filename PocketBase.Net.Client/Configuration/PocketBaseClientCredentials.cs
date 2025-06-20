namespace PocketBase.Net.Client.Configuration;

/// <summary>
/// Wraps the credentials of a PocketBase record.
/// </summary>
public record PocketBaseClientCredentials
{
    /// <summary>
    /// Username or email address of the user account to be authenticated with.
    /// </summary>
    public required string Identity { get; set; }

    public required string Password { get; set; }

    /// <summary>
    /// The collection from which this account can be authenticated
    /// (ex: <c>users</c>, <c>_superusers</c>, ...).
    /// </summary>
    /// <remarks>Set to <c>users</c> by default.</remarks>
    public string CollectionName { get; set; } = "users";
}
