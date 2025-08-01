﻿namespace PocketBase.Net.Client.Exceptions;

public sealed class RecordDeletionFailedException()
    : PocketBaseException("The deletion of the record failed")
{
    public required string RecordId { get; init; }

    public required HttpRequestMessage RequestMessage { get; init; }

    public required HttpContent Response { get; init; }
}
