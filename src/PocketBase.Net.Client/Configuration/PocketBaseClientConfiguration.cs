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
    public required PocketBaseClientCredentials ClientCredentials { get; init; }

    /// <summary>
    /// If <c>true</c>, the <see cref="IPocketBaseClient"/> will invoke <see cref="IPocketBaseClient.Authenticate"/>
    /// when first interacting with PocketBase
    /// </summary>
    public bool UseSilentAuthentication { get; set; } = true;

    /// <summary>
    /// Define the validation behavior for record operations.
    /// </summary>
    public RecordOperationBehavior RecordOperationBehavior { get; set; } = RecordOperationBehavior.Strict;

    /// <summary>
    /// Define the pipeline used to determine the name of the collection of an entity.
    /// </summary>
    public RepositoryNamingPipeline CollectionNamingPipeline { get; init; } = new();
}
