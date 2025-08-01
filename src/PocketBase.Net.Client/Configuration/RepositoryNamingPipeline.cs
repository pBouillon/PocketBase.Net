﻿using PocketBase.Net.Client.Configuration.CollectionNamingRules;
using PocketBase.Net.Client.Entities.Records;

namespace PocketBase.Net.Client.Configuration;

/// <summary>
/// Pipeline that generates collection names for record types by applying a series of naming rules.
/// Implements caching to avoid recomputing names for the same type.
/// </summary>
public sealed class RepositoryNamingPipeline
{
    private readonly IDictionary<Type, string> _nameCache = new Dictionary<Type, string>();

    private static readonly Queue<ICollectionNamingRule> _defaultNamingRulesPipeline = new([
        new SeedWithTypeName(),
        new TrimSuffix(),
        new UseCamelCase(),
        new UsePlural(),
    ]);

    private Queue<ICollectionNamingRule> _namingRulesPipeline = new(_defaultNamingRulesPipeline);

    /// <summary>
    /// Appends a new rule to the end of the naming rules pipeline.
    /// </summary>
    /// <param name="newRule">Rule to append to the pipeline.</param>
    /// <returns>The current instance to chain calls.</returns>

    public RepositoryNamingPipeline AppendRule(ICollectionNamingRule newRule)
        => AppendRules([newRule]);

    /// <summary>
    /// Appends new rules to the end of the naming rules pipeline.
    /// </summary>
    /// <param name="newRules">Rules to append to the pipeline.</param>
    /// <returns>The current instance to chain calls.</returns>
    public RepositoryNamingPipeline AppendRules(params ICollectionNamingRule[] newRules)
    {
        _namingRulesPipeline.Clear();

        newRules
            .ToList()
            .ForEach(_namingRulesPipeline.Enqueue);

        return this;
    }

    /// <summary>
    /// Gets the collection name for the specified record type.
    /// </summary>
    /// <typeparam name="TRecord">The record type to get a name for.</typeparam>
    /// <returns>The generated collection name for the record type.</returns>
    public string GetCollectionNameOf<TRecord>()
        where TRecord : RecordBase
    {
        var cacheKey = typeof(TRecord);

        var isCacheHit = _nameCache.ContainsKey(typeof(TRecord));
        if (isCacheHit)
        {
            return _nameCache[typeof(TRecord)];
        }

        var collectionName = _namingRulesPipeline.Aggregate(
            seed: string.Empty,
            (current, rule) => rule.BuildNameFor<TRecord>(current));

        _nameCache.Add(cacheKey, collectionName);

        return collectionName;
    }

    /// <summary>
    /// Replaces the current naming rules pipeline with a new set of rules.
    /// </summary>
    /// <param name="newRules">The new set of rules to use.</param>
    /// <returns>The current instance to chain calls.</returns>
    public RepositoryNamingPipeline ReplaceRulesWith(params ICollectionNamingRule[] newRules)
    {
        _namingRulesPipeline = new(newRules);
        return this;
    }
}
