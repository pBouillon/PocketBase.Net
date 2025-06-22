using PocketBase.Net.Client.Entities.Records;
using System.Collections.Immutable;

namespace PocketBase.Net.Client.Exceptions;

public sealed class UnknownPropertiesException<TRecord>()
    : PocketBaseException("One or more keys don't exist in the record definition.")
    where TRecord : RecordBase
{
    public required ImmutableArray<string> UnknownPropertiesNames { get; init; } = [];
}