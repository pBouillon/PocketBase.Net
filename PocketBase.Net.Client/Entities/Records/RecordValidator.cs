using PocketBase.Net.Client.Exceptions;
using System.Collections.Immutable;

namespace PocketBase.Net.Client.Entities.Records;

/// <summary>
/// Provides validation services for record operations.
/// </summary>
/// <typeparam name="TRecord">The record type to validate.</typeparam>
public class RecordValidator<TRecord>
    where TRecord : RecordBase
{
    public ImmutableArray<string> GetUnknownProperties(IDictionary<string, object?> payload)
    {
        var propertyDictionary = RecordPropertiesCache<TRecord>.GetPropertiesDictionary();

        return payload.Keys
            .Where(key => !propertyDictionary.ContainsKey(key))
            .ToImmutableArray();
    }

    public Dictionary<string, MismatchingTypesDetails<TRecord>> GetMismatchedPropertyTypes(IDictionary<string, object?> payload)
    {
        var propertyDictionary = RecordPropertiesCache<TRecord>.GetPropertiesDictionary();

        return payload
            .Where(kvp => kvp.Value is not null)
            .Where(kvp => propertyDictionary.ContainsKey(kvp.Key))
            .Where(kvp =>
            {
                var property = propertyDictionary[kvp.Key];
                var expectedType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                var providedType = kvp.Value!.GetType();

                return !expectedType.IsAssignableFrom(providedType);
            })
            .ToDictionary(
                kvp => kvp.Key,
                kvp => new MismatchingTypesDetails<TRecord>
                {
                    Expected = propertyDictionary[kvp.Key].PropertyType,
                    Provided = kvp.Value!.GetType()
                }
            );
    }

    public void ThrowOnMismatchedPropertyTypesIn(IDictionary<string, object?> payload)
    {
        var mismatchedProperties = GetMismatchedPropertyTypes(payload);

        if (mismatchedProperties.Count != 0)
        {
            throw new MismatchedPropertyTypesException<TRecord> { MismatchedPropertyTypes = mismatchedProperties };
        }
    }

    public void ThrowOnUnknownPropertiesIn(IDictionary<string, object?> payload)
    {
        var unknownProperties = GetUnknownProperties(payload);

        if (unknownProperties.Length != 0)
        {
            throw new UnknownPropertiesException<TRecord> { UnknownPropertiesNames = unknownProperties };
        }
    }
}