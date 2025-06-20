using System.Text.Json.Serialization;

namespace PocketBase.Net.Client.Entities.Users;

/// <summary>
/// Represent the authentication result of a <see cref="PocketBaseUser"/>.
/// </summary>
/// <typeparam name="T">The authenticated user type.</typeparam>
public record Authenticated<T> where T : PocketBaseUser
{
    /// <summary>
    /// The token used by this user to interact with PocketBase as the <see cref="User"/>.
    /// </summary>
    public string Token { get; set; } = null!;

    /// <summary>
    /// The authenticated user.
    /// </summary>
    [JsonPropertyName("record")]
    public T User { get; set; } = null!;
}
