using Microsoft.Extensions.DependencyInjection;
using PocketBase.Net.Client;

namespace PocketBase.Net.DependencyInjection;

public static class DependencyInjectionHelper
{
    public static IServiceCollection AddPocketBase(
        this IServiceCollection services,
        Func<PocketBaseClientConfiguration> createPocketBaseConfigurationFunction)
        => services
            .AddSingleton(createPocketBaseConfigurationFunction())
            .AddScoped<PocketBaseHttpClientWrapper>()
            .AddScoped<IPocketBaseClient, PocketBaseClient>();
}
