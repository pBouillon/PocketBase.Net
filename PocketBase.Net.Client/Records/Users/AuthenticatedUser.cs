using System.Text.Json.Serialization;

namespace PocketBase.Net.Client.Records.Users;

/// <summary>
/// Represent the authentication result of a <see cref="PocketBaseUser"/>.
/// </summary>
/// <typeparam name="T">The authenticated user type.</typeparam>
public record Authenticated<T> where T : PocketBaseUser
{
    public string Token { get; set; } = null!;

    [JsonPropertyName("record")]
    public T User { get; set; } = null!;
}
