namespace PocketBase.Net.Client.Entities.Records;

public record MismatchingTypesDetails<TRecord>
    where TRecord : RecordBase
{
    public required Type Expected { get; init; }
    public required Type Provided { get; init; }
}
