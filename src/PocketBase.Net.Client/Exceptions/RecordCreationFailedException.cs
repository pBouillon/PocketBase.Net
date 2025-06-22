namespace PocketBase.Net.Client.Exceptions;

public sealed class RecordCreationFailedException()
    : Exception("The creation of the new record failed")
{
    public required object? Payload { get; init; }

    public required HttpRequestMessage RequestMessage { get; init; }

    public required HttpContent Response { get; init; }
}
