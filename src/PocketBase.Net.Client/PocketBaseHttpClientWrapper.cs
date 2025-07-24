using PocketBase.Net.Client.Configuration;
using PocketBase.Net.Client.Entities;
using PocketBase.Net.Client.Entities.Records;
using PocketBase.Net.Client.Entities.Users;
using PocketBase.Net.Client.Exceptions;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace PocketBase.Net.Client;

/// <summary>
/// PocketBase HTTP client wrapper that manages its authentication status, default endpoints and serialization.
/// </summary>
/// <param name="configuration">The initial configuration, seeding the dependencies options.</param>
public sealed class PocketBaseHttpClientWrapper(PocketBaseClientConfiguration configuration)
{
    /// <summary>
    /// The wrapped <see cref="HttpClient"/>.
    /// </summary>
    private readonly HttpClient _httpClient = new() { BaseAddress = configuration.ServerUrl };

    /// <summary>
    /// Serialization options when emitting HTTP requests to the PocketBase API.
    /// </summary>
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new PocketBaseDateTimeConverter() },
    };

    /// <summary>
    /// Indicates if the client is currently authenticated based on the existence of the <c>Authorization</c> request header.
    /// </summary>
    public bool IsAuthenticated => _httpClient.DefaultRequestHeaders.Authorization is not null;

    /// <summary>
    /// Appends query parameters to a root URL string.
    /// </summary>
    /// <param name="root">The base URL.</param>
    /// <param name="queryParameters">Query parameters to append, with null values filtered out.</param>
    /// <returns>The root URL with appended query parameters.</returns>
    private static string AppendQueryParameters(string root, params string?[] queryParameters)
    {
        var toAppend = queryParameters
            .Where(queryParameter => !string.IsNullOrEmpty(queryParameter))
            .ToArray();

        return toAppend.Length == 0
            ? root
            : root + '?' + toAppend[1..].Aggregate(
                seed: toAppend[0],
                (current, queryParameter) => $"{current}&{queryParameter}");
    }

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
            throw new AuthenticationFailedException
            {
                Response = response.Content,
                StatusCode = response.StatusCode,
            };
        }

        var rawContent = await response.Content.ReadAsStringAsync(cancellationToken);

        var authenticated = JsonSerializer.Deserialize<Authenticated<PocketBaseUser>>(rawContent, _jsonSerializerOptions)
            ?? throw new MalformedResponseException<Authenticated<PocketBaseUser>> { Received = rawContent };

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticated.Token);

        return authenticated.User;
    }

    /// <summary>
    /// Attempt to authenticate the client according to the configuration behavior.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The authenticated <see cref="PocketBaseUser"/>.</returns>
    /// <exception cref="UnauthenticatedClientException"></exception>
    private Task<PocketBaseUser> HandleUnauthenticatedClient(CancellationToken cancellationToken)
        => configuration.UseSilentAuthentication
            ? AuthenticateUsing(configuration.ClientCredentials, cancellationToken)
            : throw new UnauthenticatedClientException();

    /// <summary>
    /// Send a DELETE request to a collection to delete an existing record.
    /// </summary>
    /// <param name="collectionIdOrName">The collection id or name in which the record will be deleted.</param>
    /// <param name="recordId">The id of the record to delete..</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    public Task SendDelete(
        string collectionIdOrName,
        string recordId,
        CancellationToken cancellationToken)
        => SendRequest(
                (httpClient) => httpClient.DeleteAsync(
                    $"/api/collections/{collectionIdOrName}/records/{recordId}",
                    cancellationToken),
                onErrorThrow: (response) => new RecordDeletionFailedException
                {
                    RecordId = recordId,
                    RequestMessage = response.RequestMessage!,
                    Response = response.Content,
                }, cancellationToken);

    /// <summary>
    /// Send a GET request to a collection to retrieve an existing record
    /// </summary>
    /// <typeparam name="TRecord">The type of the expected response.</typeparam>
    /// <param name="collectionIdOrName">The collection id or name in which the record is.</param>
    /// <param name="recordId">The id of the record to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The retrieved <typeparamref name="TRecord"/>.</returns>
    public Task<TRecord> SendGetById<TRecord>(
        string collectionIdOrName,
        string recordId,
        CancellationToken cancellationToken = default
     ) where TRecord : RecordBase
        => SendRequest<TRecord>(
            (httpClient) => httpClient.GetAsync(
                $"/api/collections/{collectionIdOrName}/records/{recordId}",
                cancellationToken),
            onErrorThrow: (_) => new RecordSearchFailedException(),
            cancellationToken);

public Task<Paged<TRecord>> SendGet<TRecord>(
    string collectionIdOrName,
    string? filter = null,
    PaginationOptions? paginationOptions = null,
    string? sorting = null,
    CancellationToken cancellationToken = default
) where TRecord : RecordBase
{
    var query = AppendQueryParameters(
        $"/api/collections/{collectionIdOrName}/records",
        string.IsNullOrEmpty(filter) ? null : $"filter=({filter})",
        string.IsNullOrEmpty(sorting) ? null : $"sort={sorting}",
        paginationOptions?.ToQueryParameters());

    return SendRequest<Paged<TRecord>>(
            (httpClient) => httpClient.GetAsync(query, cancellationToken),
            onErrorThrow: (_) => new RecordSearchFailedException(),
            cancellationToken);
}

    /// <summary>
    /// Send a PATCH request to a collection to update an existing record
    /// </summary>
    /// <typeparam name="TRecord">The type of the expected response.</typeparam>
    /// <param name="collectionIdOrName">The collection id or name in which the record will be updated.</param>
    /// <param name="recordId">The id of the record to update.</param>
    /// <param name="payload">The content to be serialized as the body of the PATCH request.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The updated <typeparamref name="TRecord"/>.</returns>
    public Task<TRecord> SendPatch<TRecord>(
        string collectionIdOrName,
        string recordId,
        IDictionary<string, object?> payload,
        CancellationToken cancellationToken
    ) where TRecord : RecordBase
        => SendRequest<TRecord>(
            (httpClient) => httpClient.PatchAsJsonAsync(
               $"/api/collections/{collectionIdOrName}/records/{recordId}",
               payload,
               _jsonSerializerOptions,
               cancellationToken
            ),
            onErrorThrow: (response) => new RecordUpdateFailedException
            {
                RecordId = recordId,
                Payload = payload,
                RequestMessage = response.RequestMessage!,
                Response = response.Content,
            },
            cancellationToken);

    /// <summary>
    /// Send a POST request to a collection to create a new record.
    /// </summary>
    /// <typeparam name="TPayload">The type of the payload to send in the body of the POST request.</typeparam>
    /// <typeparam name="TRecord">The type of the expected response.</typeparam>
    /// <param name="collectionIdOrName">The collection id or name in which the record will be created.</param>
    /// <param name="payload">The content to be serialized as the body of the POST request.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The newly created <typeparamref name="TRecord"/> on success.</returns>
    public Task<TRecord> SendPost<TPayload, TRecord>(
        string collectionIdOrName,
        TPayload payload,
        CancellationToken cancellationToken
    ) where TRecord : RecordBase
        => SendRequest<TRecord>(
            (httpClient) => httpClient.PostAsJsonAsync(
               $"/api/collections/{collectionIdOrName}/records",
               payload,
               _jsonSerializerOptions,
               cancellationToken),
            onErrorThrow: (response) => new RecordCreationFailedException
            {
                Payload = payload,
                RequestMessage = response.RequestMessage!,
                Response = response.Content,
            },
            cancellationToken);

    /// <summary>
    /// Sends an HTTP request, ensuring the client is authenticated, and handles error responses.
    /// </summary>
    /// <param name="request">A delegate that takes an <see cref="HttpClient"/> and returns a task representing the HTTP response message.</param>
    /// <param name="onErrorThrow">A delegate that takes the failed <see cref="HttpResponseMessage"/> and returns an appropriate <see cref="PocketBaseException"/> to throw.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <remarks>
    /// If the client is not authenticated, it will attempt to authenticate based on the configuration before sending the request.
    /// </remarks>
    private async Task SendRequest(
        Func<HttpClient, Task<HttpResponseMessage>> request,
        Func<HttpResponseMessage, PocketBaseException> onErrorThrow,
        CancellationToken cancellationToken)
    {
        if (!IsAuthenticated)
        {
            await HandleUnauthenticatedClient(cancellationToken);
        }
        using var response = await request.Invoke(_httpClient);
        if (!response.IsSuccessStatusCode)
        {
            throw onErrorThrow.Invoke(response);
        }
    }

    /// <summary>
    /// Sends an HTTP request, ensures the client is authenticated, handles error responses, and deserializes the response content.
    /// </summary>
    /// <typeparam name="TDeserialized">The type of the expected response after deserialization.</typeparam>
    /// <param name="request">A delegate that takes an <see cref="HttpClient"/> and returns a task representing the HTTP response message.</param>
    /// <param name="onErrorThrow">A delegate that takes the failed <see cref="HttpResponseMessage"/> and returns an appropriate <see cref="PocketBaseException"/> to throw.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the deserialized response of type <typeparamref name="TDeserialized"/>.</returns>
    /// <remarks>
    /// If the client is not authenticated, it will attempt to authenticate based on the configuration before sending the request.
    /// </remarks>
    private async Task<TDeserialized> SendRequest<TDeserialized>(
        Func<HttpClient, Task<HttpResponseMessage>> request,
        Func<HttpResponseMessage, PocketBaseException> onErrorThrow,
        CancellationToken cancellationToken)
    {
        if (!IsAuthenticated)
        {
            await HandleUnauthenticatedClient(cancellationToken);
        }

        using var response = await request.Invoke(_httpClient);

        if (!response.IsSuccessStatusCode)
        {
            throw onErrorThrow.Invoke(response);
        }

        var rawContent = await response.Content.ReadAsStringAsync(cancellationToken);

        return JsonSerializer.Deserialize<TDeserialized>(rawContent, _jsonSerializerOptions)
            ?? throw new MalformedResponseException<TDeserialized> { Received = rawContent };
    }
}
