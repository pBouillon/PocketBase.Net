namespace PocketBase.Net.Client.Configuration.CollectionNamingRules;

/// <summary>
/// Apply a transformation to the name.
/// </summary>
public sealed class UseTransformation(Func<string, string> Transformation) : ICollectionNamingRule
{
    /// <summary>
    /// Apply the given<see cref="UseTransformation.Transformation"/> to the <see paramref="currentName"/>.
    /// </summary>
    /// <typeparam name="TRecord">The record type being named.</typeparam>
    /// <param name="currentName">The current name to process.</param>
    /// <returns>The transformed name.</returns>
    public string BuildNameFor<TRecord>(string currentName)
        => Transformation.Invoke(currentName);
}
