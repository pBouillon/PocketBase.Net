using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.DependencyInjection;
using PocketBase.Net.Client.Configuration;
using PocketBase.Net.Client.IntegrationTests.Utilities;
using PocketBase.Net.DependencyInjection;

namespace PocketBase.Net.Client.IntegrationTests.Fixtures;

public class PocketBaseFixture : IAsyncLifetime
{
    private const int PocketBasePort = 8090;

    public readonly PocketBaseClientCredentials AdminCredentials = new()
    {
        Identity = "admin@localhost.com",
        Password = "please don't hack me",
        CollectionName = "_superusers",
    };

    private readonly IContainer _pocketBaseContainer;

    public IServiceProvider ServiceProvider { get; private set; }
        = new ServiceCollection().BuildServiceProvider();

    public PocketBaseFixture()
    {
        var migrationsDirectory = Path.Combine(
            CommonDirectoryPath.GetSolutionDirectory().DirectoryPath,
            "tests",
            "PocketBase.Net.Client.IntegrationTests",
            "PocketBaseMigrations");

        _pocketBaseContainer = new ContainerBuilder()
            .WithImage("adrianmusante/pocketbase:0.28.4")
            .WithPortBinding(PocketBasePort, true)
            .WithWaitStrategy(
                Wait.ForUnixContainer()
                    .UntilHttpRequestIsSucceeded(r => r
                        .ForPort(PocketBasePort)
                        .ForPath("/api/health")
                        .WithMethod(HttpMethod.Get)))
            .WithCleanUp(true)
            .WithEnvironment("POCKETBASE_ADMIN_EMAIL", AdminCredentials.Identity)
            .WithEnvironment("POCKETBASE_ADMIN_PASSWORD", AdminCredentials.Password)
            .WithResourceMapping(
                source: migrationsDirectory,
                target: "/pocketbase/migrations")
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _pocketBaseContainer
            .StartAsync()
            .ConfigureAwait(false);

        var pocketBaseBaseUrl = 
            $"http://{_pocketBaseContainer.Hostname}:" +
            $"{_pocketBaseContainer.GetMappedPublicPort(PocketBasePort)}";

        ServiceProvider = new ServiceCollection()
            .AddPocketBase(
                serverUrl: new Uri(pocketBaseBaseUrl),
                credentials: AdminCredentials)
            .AddPocketBaseRepositories(scanningAssembly: typeof(TodoItemRecord).Assembly)
            .BuildServiceProvider();
    }

    public async Task DisposeAsync()
    {
        await _pocketBaseContainer
            .StopAsync()
            .ConfigureAwait(false);

        await _pocketBaseContainer
            .DisposeAsync()
            .ConfigureAwait(false);
    }
}
