using Microsoft.Extensions.DependencyInjection;
using PocketBase.Net.Client;
using PocketBase.Net.Client.Configuration;
using PocketBase.Net.Client.Entities.Records;
using PocketBase.Net.Client.Entities.Repository;
using System.Collections.Immutable;
using System.Reflection;

namespace PocketBase.Net.DependencyInjection;

/// <summary>
/// Helper class for setting up dependency injection for <see cref="PocketBaseClient"/> services and repositories.
/// </summary>
public static class DependencyInjectionHelper
{
    /// <summary>
    /// Adds <see cref="PocketBaseClient"/> and related services to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add to.</param>
    /// <param name="serverUrl">The URL on which the PocketBase instance is running.</param>
    /// <param name="credentials">The credentials to be used to authenticate to the PocketBase instance.</param>
    /// <param name="mutation">An optional configuration of the <see cref="PocketBaseClientConfiguration"/>.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddPocketBase(
        this IServiceCollection services,
        Uri serverUrl,
        PocketBaseClientCredentials credentials,
        Action<PocketBaseClientConfiguration>? mutation = null)
    {
        var configuration = new PocketBaseClientConfiguration
        {
            ServerUrl = serverUrl,
            ClientCredentials = credentials,
        };

        mutation?.Invoke(configuration);

        return services
            .AddSingleton(configuration)
            .AddScoped<PocketBaseHttpClientWrapper>()
            .AddScoped<IPocketBaseClient, PocketBaseClient>();
    }

    /// <summary>
    /// Adds PocketBase repositories to the service collection by scanning the specified assembly.
    /// </summary>
    /// <param name="services">The service collection to add to.</param>
    /// <param name="scanningAssembly">The assembly to scan for record types. Defaults to the executing assembly.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddPocketBaseRepositories(
        this IServiceCollection services,
        Assembly? scanningAssembly = null)
    {
        scanningAssembly ??= Assembly.GetExecutingAssembly();

        var configuration = (PocketBaseClientConfiguration)(services
            .FirstOrDefault(x => x.ServiceType == typeof(PocketBaseClientConfiguration))
            ?.ImplementationInstance
            ?? throw new Exception("Unable to retrieve the client configuration"));

        return services
            .AddRecordRepositories(scanningAssembly)
            .AddRecordValidators(configuration.RecordOperationBehavior, scanningAssembly);
    }

    /// <summary>
    /// Discovers and registers all record repositories for record types in the specified assembly.
    /// </summary>
    /// <param name="services">The service collection to add to.</param>
    /// <param name="scanningAssembly">The assembly to scan for record types.</param>
    /// <returns>The service collection for chaining.</returns>
    private static IServiceCollection AddRecordRepositories(
        this IServiceCollection services,
        Assembly scanningAssembly)
    {
        var recordTypes = scanningAssembly
            .GetTypes()
            .Where(t => t.IsClass
                && !t.IsAbstract
                && typeof(RecordBase).IsAssignableFrom(t));

        var recordRepositoryTypes = recordTypes
            .Where(recordType => recordType.GetCustomAttribute<SkipRepositoryCreationAttribute>() is null)
            .Select(recordType => new
            {
                RepositoryInterface = typeof(IRepository<>).MakeGenericType(recordType),
                RepositoryType = typeof(Repository<>).MakeGenericType(recordType),
            })
            .ToImmutableList();

        recordRepositoryTypes
            .ForEach(x => services.AddScoped(x.RepositoryInterface, x.RepositoryType));

        return services;
    }

    /// <summary>
    /// Discovers and registers all <see cref="RecordValidator{TRecord}"/> services for record types in the calling assembly.
    /// </summary>
    /// <param name="services">The service collection to add to.</param>
    /// <param name="recordOperationBehavior">The behavior configuration for record operations.</param>
    /// <param name="scanningAssembly">The assembly to scan for record types. Defaults to calling assembly.</param>
    /// <returns>The service collection for chaining.</returns>
    private static IServiceCollection AddRecordValidators(
        this IServiceCollection services,
        RecordOperationBehavior recordOperationBehavior,
        Assembly scanningAssembly)
    {
        // If validation is disabled, don't bloat the DI container with validators
        var shouldSkipAllValidation = recordOperationBehavior == RecordOperationBehavior.IgnoreAll;
        if (shouldSkipAllValidation)
        {
            return services;
        }

        var recordTypes = scanningAssembly
            .GetTypes()
            .Where(t => t.IsClass
                && !t.IsAbstract
                && typeof(RecordBase).IsAssignableFrom(t));

        var recordValidatorTypes = recordTypes
            .Select(recordType => typeof(RecordValidator<>).MakeGenericType(recordType))
            .ToImmutableList();

        recordValidatorTypes.ForEach(type => services.AddScoped(type));

        return services;
    }
}
