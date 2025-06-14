namespace PocketBase.Net.Client;

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

/// <summary>
/// Configuration object to fine tune the behavior of the <see cref="PocketBaseClient"/>
/// </summary>
public record PocketBaseClientConfiguration
{
    /// <summary>
    /// The base URI of the running PocketBase instance (ex: <c>"http://localhost:8090"</c>).
    /// </summary>
    public required Uri ServerUrl { get; init; }

    /// <summary>
    /// The <see cref="PocketBaseClientCredentials"/> to be used to authenticate the <see cref="PocketBaseClient"/>.
    /// </summary>
    public required PocketBaseClientCredentials ClientCredentials { get; set; }
}
