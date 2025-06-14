using Microsoft.Extensions.DependencyInjection;
using PocketBase.Net.Client;

namespace PocketBase.Net.DependencyInjection;

public static class DependencyInjectionHelper
{
    public static IServiceCollection AddPocketBase(
        this IServiceCollection services,
        Func<PocketBaseClientConfiguration> configurationAction)
    {
        var pocketBaseConfiguration = configurationAction();

        services.AddScoped<IPocketBaseClient>((_) => new PocketBaseClient(pocketBaseConfiguration));

        return services;
    }
}
