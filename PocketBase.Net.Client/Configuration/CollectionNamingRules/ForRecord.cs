using PocketBase.Net.Client.Entities.Records;

namespace PocketBase.Net.Client.Configuration.CollectionNamingRules;

/// <summary>
/// Rule that applies a custom transformation for a specific record type.
/// </summary>
/// <typeparam name="TTargetedRecord">The specific record type this rule applies to.</typeparam>
public sealed class ForRecord<TTargetedRecord>(Func<string, string> Transformation)
    : ICollectionNamingRule
    where TTargetedRecord : RecordBase
{
    /// <summary>
    /// Applies the custom transformation if this rule targets the current record type.
    /// </summary>
    /// <typeparam name="TRecord">The record type being named.</typeparam>
    /// <param name="currentName">The current name to potentially transform.</param>
    /// <returns>
    /// The transformed name if this rule applies to the current type,
    /// otherwise the original name.
    /// </returns> name="string" />
    public string BuildNameFor<TRecord>(string currentName)
        => typeof(TTargetedRecord) == typeof(TRecord)
            ? Transformation.Invoke(currentName)
            : currentName;
}
