namespace PocketBase.Net.Client.Exceptions;

public sealed class UnauthenticatedClientException()
    : PocketBaseException("The client is not yet authenticated.")
{ }
