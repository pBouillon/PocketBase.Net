namespace PocketBase.Net.Client.Configuration.CollectionNamingRules;

/// <summary>
/// Rule that seeds the name with the type name of the record.
/// </summary>
public sealed class SeedWithTypeName : ICollectionNamingRule
{
    /// <summary>
    /// Seeds the name with the type name of the record.
    /// </summary>
    /// <typeparam name="TRecord">The record type being named.</typeparam>
    /// <param name="currentName">The current name (ignored for seeding).</param>
    /// <returns>The type name converted to camelCase.</returns>
    public string BuildNameFor<TRecord>(string currentName)
    {
        var typeName = typeof(TRecord).Name;
        return char.ToLowerInvariant(typeName[0]) + typeName[1..];
    }
}
