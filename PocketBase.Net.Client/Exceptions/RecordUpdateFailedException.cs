namespace PocketBase.Net.Client.Exceptions;

public sealed class RecordUpdateFailedException()
    : Exception("The update of the record failed")
{
    public required string RecordId { get; init; }

    public required object? Payload { get; init; }

    public required HttpRequestMessage RequestMessage { get; init; }

    public required HttpContent Response { get; init; }
}