using Microsoft.Extensions.DependencyInjection;
using PocketBase.Net.Client.Configuration;
using PocketBase.Net.Client.Exceptions;
using PocketBase.Net.Client.IntegrationTests.Fixtures;
using Shouldly;
using System.Net;

namespace PocketBase.Net.Client.IntegrationTests.Features;

public class AuthenticationTests(PocketBaseFixture fixture)
    : IClassFixture<PocketBaseFixture>
{
    [Trait("scenario", "failure")]
    [Fact(DisplayName = "PocketBase API should reject authentication attempts with invalid credentials")]
    public async Task AuthenticateUsing_WithInvalidCredentials_ShouldFail()
    {
        // Arrange
        var invalidCredentials = new PocketBaseClientCredentials
        {
            Identity = "Leroy Jenkins",
            Password = "Leeeeeeeeerooooooooy",
        };

        var client = fixture.ServiceProvider.GetRequiredService<PocketBaseHttpClientWrapper>();

        // Act & Assert
        var exception = await Should.ThrowAsync<AuthenticationFailedException>(
            () => client.AuthenticateUsing(invalidCredentials, CancellationToken.None));

        exception.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Trait("scenario", "happy path")]
    [Fact(DisplayName = "Authentication should succeed with valid admin credentials")]
    public async Task AuthenticateUsing_WithValidCredentials_ShouldReturnAuthenticatedUser()
    {
        // Arrange
        var client = fixture.ServiceProvider.GetRequiredService<PocketBaseHttpClientWrapper>();

        // Act
        var user = await client.AuthenticateUsing(
            fixture.AdminCredentials,
            CancellationToken.None);

        // Assert
        user.ShouldNotBeNull();
        user.Email.ShouldBe(fixture.AdminCredentials.Identity);
        client.IsAuthenticated.ShouldBeTrue();
    }
}
