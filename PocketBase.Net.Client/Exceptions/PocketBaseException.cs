
namespace PocketBase.Net.Client.Exceptions;

public class PocketBaseException(string message, Exception? innerException = null)
    : Exception(message, innerException)
{ }
