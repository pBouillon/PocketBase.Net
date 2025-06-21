namespace PocketBase.Net.Client.Configuration.CollectionNamingRules;

/// <summary>
/// Rule that ensures the name is plural.
/// </summary>
public sealed class UsePlural : ICollectionNamingRule
{
    /// <summary>
    /// Appends an 's' to the name if it doesn't already end with 's'.
    /// </summary>
    /// <typeparam name="TRecord">The record type being named.</typeparam>
    /// <param name="currentName">The current name to process.</param>
    /// <returns>The name with pluralization applied.</returns>
    public string BuildNameFor<TRecord>(string currentName)
        => currentName.EndsWith('s')
            ? currentName
            : currentName + 's';
}
