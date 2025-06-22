using System.Collections.Concurrent;
using System.Reflection;

namespace PocketBase.Net.Client.Entities.Records;

/// <summary>
/// Provides cached access to property information for record types inheriting from <see cref="RecordBase"/>.
/// </summary>
/// <typeparam name="TRecord">The record type whose properties are to be cached.</typeparam>
internal static class RecordPropertiesCache<TRecord>
    where TRecord : RecordBase
{
    /// <summary>
    /// Provides cached access to property information for record types inheriting from <see cref="RecordBase"/>.
    /// </summary>
    /// <typeparam name="TRecord">The record type whose properties are to be cached.</typeparam>
    private static readonly ConcurrentDictionary<Type, Dictionary<string, PropertyInfo>> PropertiesCache = new();

    /// <summary>
    /// Gets a dictionary of property information for the generic type parameter <typeparamref name="TRecord"/>.
    /// </summary>
    /// <returns>
    /// A dictionary mapping property names (case-insensitive) to their corresponding <see cref="PropertyInfo"/>
    /// objects for type <typeparamref name="TRecord"/>.
    /// </returns>
    /// <remarks>
    /// The dictionary is cached after the first call, so subsequent calls are served from the cache.
    /// Property names in the dictionary are compared case-insensitively.
    /// </remarks>
    public static Dictionary<string, PropertyInfo> GetPropertiesDictionary()
        => PropertiesCache.GetOrAdd(
            typeof(TRecord),
            type => type.GetProperties().ToDictionary(
                p => p.Name,
                p => p,
                StringComparer.OrdinalIgnoreCase));
}
