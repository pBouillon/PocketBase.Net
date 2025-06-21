using System.Text.RegularExpressions;

namespace PocketBase.Net.Client.Configuration.CollectionNamingRules;

/// <summary>
/// Rule that removes a specified suffix from the name.
/// </summary>
public sealed class TrimSuffix(string Suffix = "Record") : ICollectionNamingRule
{
    /// <summary>
    /// Removes the specified suffix from the current name.
    /// </summary>
    /// <typeparam name="TRecord">The record type being named.</typeparam>
    /// <param name="currentName">The current name to process.</param>
    /// <returns>The name with the suffix removed.</returns>
    public string BuildNameFor<TRecord>(string currentName)
        => Regex.Replace(currentName, $@"{Suffix}$", string.Empty);
}
