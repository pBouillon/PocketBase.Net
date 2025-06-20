using PocketBase.Net.Client.Entities.Traits;
using System.Text.Json.Serialization;

namespace PocketBase.Net.Client.Entities.Users;

/// <summary>
/// Represent a user in PocketBase
/// </summary>
public record PocketBaseUser : IIdentifiable, IAuditable
{
    /// <inheritdoc/>
    public string Id { get; set; } = null!;

    /// <inheritdoc/>
    public DateTime Created { get; set; }

    /// <inheritdoc/>
    public DateTime Updated { get; set; }

    /// <summary>
    /// The username of this account.
    /// </summary>
    public string? Username { get; set; } = null!;

    /// <summary>
    /// The associated email address.
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// Indicates if this user account has been verified.
    /// </summary>
    [JsonPropertyName("verified")]
    public bool IsVerified { get; set; }

    /// <summary>
    /// Indicates if the email address of this user is public or private.
    /// </summary>
    [JsonPropertyName("emailVisibility")]
    public bool IsEmailPublic { get; set; }
}
