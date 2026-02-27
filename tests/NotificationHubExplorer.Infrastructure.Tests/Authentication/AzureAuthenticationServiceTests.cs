using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NotificationHubExplorer.Infrastructure.Authentication;

namespace NotificationHubExplorer.Infrastructure.Tests.Authentication;

[TestClass]
public class AzureAuthenticationServiceTests
{
    [TestMethod]
    public void IsAuthenticated_Should_ReturnFalse_When_AuthenticateHasNotBeenCalled()
    {
        var service = new AzureAuthenticationService();

        service.IsAuthenticated.Should().BeFalse();
    }

    [TestMethod]
    public async Task IsAuthenticated_Should_ReturnTrue_When_AuthenticateAsyncCompletes()
    {
        var service = new AzureAuthenticationService();

        await service.AuthenticateAsync(CancellationToken.None);

        service.IsAuthenticated.Should().BeTrue();
    }

    [TestMethod]
    public void GetCredential_Should_ThrowInvalidOperationException_When_NotAuthenticated()
    {
        var service = new AzureAuthenticationService();

        var act = () => service.GetCredential();

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*authenticated*");
    }

    [TestMethod]
    public async Task GetCredential_Should_ReturnCredential_When_Authenticated()
    {
        var service = new AzureAuthenticationService();
        await service.AuthenticateAsync(CancellationToken.None);

        var credential = service.GetCredential();

        credential.Should().NotBeNull();
    }
}
