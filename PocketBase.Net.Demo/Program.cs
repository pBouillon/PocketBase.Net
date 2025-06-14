using Microsoft.Extensions.DependencyInjection;
using PocketBase.Net.Client;
using PocketBase.Net.DependencyInjection;

var services = new ServiceCollection();

services.AddPocketBase(() => new PocketBaseClientConfiguration
{
    ServerUrl = new Uri("http://localhost:8090"),
    ClientCredentials = new PocketBaseClientCredentials
    {
        Identity = "technical@account.com",
        Password = "PleaseDontHackMe",
        CollectionName = "_superusers",
    },
});

var container = services.BuildServiceProvider();

var pocketBaseClient = container.GetRequiredService<IPocketBaseClient>();
var user = await pocketBaseClient.Authenticate();
