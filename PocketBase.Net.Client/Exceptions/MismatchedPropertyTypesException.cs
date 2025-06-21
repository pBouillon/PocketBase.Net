using PocketBase.Net.Client.Entities.Records;

namespace PocketBase.Net.Client.Exceptions;

public class MismatchedPropertyTypesException<TRecord>()
    : PocketBaseException("One or more properties have an unexpected type.")
    where TRecord : RecordBase
{
    public required Dictionary<string, MismatchingTypesDetails<TRecord>> MismatchedPropertyTypes { get; init; }
}
