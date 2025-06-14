using PocketBase.Net.Client.Records.Users;

namespace PocketBase.Net.Client;

public interface IPocketBaseClient
{
    Task<PocketBaseUser> Authenticate(CancellationToken cancellation = default);
}

public class PocketBaseClient(PocketBaseClientConfiguration configuration) : IPocketBaseClient
{
    private readonly HttpClientWrapper _httpClientWrapper = new(configuration);

    public Task<PocketBaseUser> Authenticate(CancellationToken cancellation = default)
        => _httpClientWrapper.AuthenticateUsing(configuration.ClientCredentials, cancellation);
}
