namespace PocketBase.Net.Client.Configuration;

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

    /// <summary>
    /// If <c>true</c>, the <see cref="IPocketBaseClient"/> will invoke <see cref="IPocketBaseClient.Authenticate"/>
    /// when first interacting with PocketBase
    /// </summary>
    public bool UseSilentAuthentication { get; set; } = true;
}
