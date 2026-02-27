namespace NotificationHubExplorer.Core.Interfaces;

public interface IAzureAuthenticationService
{
    Task AuthenticateAsync(CancellationToken cancellationToken);
    bool IsAuthenticated { get; }
}
