using PocketBase.Net.Client.Configuration;
using PocketBase.Net.Client.Entities;
using PocketBase.Net.Client.Entities.Users;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace PocketBase.Net.Client;

/// <summary>
/// PocketBase HTTP client wrapper that manages its authentication status, default endpoints and serialization.
/// </summary>
/// <param name="configuration">The initial configuration, seeding the dependencie's options.</param>
public sealed class PocketBaseHttpClientWrapper(PocketBaseClientConfiguration configuration)
{
    /// <summary>
    /// The wrapped <see cref="HttpClient"/>.
    /// </summary>
    private readonly HttpClient _httpClient = new() { BaseAddress = configuration.ServerUrl };

    /// <summary>
    /// Serialization options when emitting HTTP requests to the PocketBase API.
    /// </summary>
    // TODO - Allow customization in DI
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new PocketBaseDateTimeConverter() },
    };

    /// <summary>
    /// Indicates if the client is currently authenticated based on the existence of the <c>Authorization</c> request header.
    /// </summary>
    public bool IsAuthenticated => _httpClient.DefaultRequestHeaders.Authorization is not null;

    /// <summary>
    /// Authenticate the current <see cref="PocketBaseHttpClientWrapper"/> with the provided <paramref name="credentials"/>.
    /// </summary>
    /// <param name="credentials">The credentials to be used to authenticate the client.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The authenticated <see cref="PocketBaseUser"/> on success.</returns>
    /// <remarks>
    /// The <c>Bearer</c> header is cleared upon calling this method, and set if the authentication is successful.
    /// </remarks>
    public async Task<PocketBaseUser> AuthenticateUsing(
        PocketBaseClientCredentials credentials,
        CancellationToken cancellationToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization = null;

        using var response = await _httpClient.PostAsJsonAsync(
           $"/api/collections/{credentials.CollectionName}/auth-with-password",
           credentials,
           _jsonSerializerOptions,
           cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            // TODO - Error handling
        }

        var rawContent = await response.Content.ReadAsStringAsync(cancellationToken);
        var authenticationResult = JsonSerializer.Deserialize<Authenticated<PocketBaseUser>>(rawContent, _jsonSerializerOptions);

        if (authenticationResult is null)
        {
            // TODO - Error handling
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticationResult.Token);

        return authenticationResult.User;
    }

    /// <summary>
    /// Send a POST request to a collection to create a new record.
    /// </summary>
    /// <typeparam name="TPayload">The type of the payload to send in the body of the POST request.</typeparam>
    /// <typeparam name="TRecord">The type of the expected response.</typeparam>
    /// <param name="collectionIdOrName">The collection id or name in which the record will be created.</param>
    /// <param name="payload">The content to be serialized as the body of the POST request.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The newly created <typeparamref name="TRecord"/> on success.</returns>
    public async Task<TRecord> CreateRecord<TPayload, TRecord>(
        string collectionIdOrName,
        TPayload payload,
        CancellationToken cancellationToken
    ) where TRecord : RecordBase
    {
        if (!IsAuthenticated)
        {
            await HandleUnauthenticatedClient(cancellationToken);
        }

        using var response = await _httpClient.PostAsJsonAsync(
           $"/api/collections/{collectionIdOrName}/records",
           payload,
           _jsonSerializerOptions,
           cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            // TODO - Error handling
        }

        var rawContent = await response.Content.ReadAsStringAsync(cancellationToken);

        // TODO - Strengthen error check
        return JsonSerializer.Deserialize<TRecord>(rawContent, _jsonSerializerOptions);
    }

    private Task<PocketBaseUser> HandleUnauthenticatedClient(CancellationToken cancellationToken)
        => configuration.UseSilentAuthentication
            ? AuthenticateUsing(configuration.ClientCredentials, cancellationToken)
            // TODO - Custom Exception
            : throw new Exception();
}
