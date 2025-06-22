namespace PocketBase.Net.Client.Configuration.CollectionNamingRules;

/// <summary>
/// Rule that converts the name to camelCase.
/// </summary>
public sealed class UseCamelCase : ICollectionNamingRule
{
    /// <summary>
    /// Converts the name to camelCase.
    /// </summary>
    /// <typeparam name="TRecord">The record type being named.</typeparam>
    /// <param name="currentName">The current name to convert.</param>
    /// <returns>The name in camelCase format.</returns>
    public string BuildNameFor<TRecord>(string currentName)
        => char.ToLowerInvariant(currentName[0]) + currentName[1..];
}
