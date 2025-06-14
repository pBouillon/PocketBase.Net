using PocketBase.Net.Client.Records.Users;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace PocketBase.Net.Client;

/// <summary>
/// PocketBase HTTP client wrapper that manages its authentication status, default endpoints and serialization.
/// </summary>
/// <param name="configuration">The initial configuration, seeding the dependencie's options.</param>
internal sealed class HttpClientWrapper
{
    /// <summary>
    /// The wrapped <see cref="HttpClient"/>.
    /// </summary>
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Serialization options when emitting HTTP requests to the PocketBase API.
    /// </summary>
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new PocketBaseDateTimeConverter(), },
    };

    /// <summary>
    /// Indicates if the client is currently authenticated based on the existence of the <c>Authorization</c> request header.
    /// </summary>
    public bool IsAuthenticated => _httpClient.DefaultRequestHeaders.Authorization is not null;

    public HttpClientWrapper(PocketBaseClientConfiguration configuration)
    {
        _httpClient = new() { BaseAddress = configuration.ServerUrl };
    }

    /// <summary>
    /// Authenticate the current <see cref="HttpClientWrapper"/> with the provided <paramref name="credentials"/>.
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
}
