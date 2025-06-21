namespace PocketBase.Net.Client.Exceptions;

public sealed class MalformedResponseException<TExpected>()
    : PocketBaseException($"Unable to read a {nameof(TExpected)} from the received response")
{
    public required string Received { get; init; } = string.Empty;
}