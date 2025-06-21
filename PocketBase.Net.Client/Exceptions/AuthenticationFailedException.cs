using System.Net;

namespace PocketBase.Net.Client.Exceptions;

public sealed class AuthenticationFailedException()
    : PocketBaseException("Unable to authenticate the client with the provided credentials.")
{
    public required HttpContent Response { get; init; }

    public required HttpStatusCode StatusCode { get; init; }
}
