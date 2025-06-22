namespace PocketBase.Net.Client.Configuration;

/// <summary>
/// Interface for rules that can transform a collection name during the naming pipeline process.
/// </summary>
public interface ICollectionNamingRule
{
    /// <summary>
    /// Applies this naming rule to the current name.
    /// </summary>
    /// <typeparam name="TRecord">The record type being named.</typeparam>
    /// <param name="currentName">The current name being built.</param>
    /// <returns>The transformed name after applying this rule.</returns>
    string BuildNameFor<TRecord>(string currentName);
}
