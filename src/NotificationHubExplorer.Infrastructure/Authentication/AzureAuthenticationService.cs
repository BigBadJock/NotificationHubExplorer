using Azure.Core;
using Azure.Identity;
using NotificationHubExplorer.Core.Interfaces;

namespace NotificationHubExplorer.Infrastructure.Authentication;

public sealed class AzureAuthenticationService : IAzureAuthenticationService
{
    private TokenCredential? _credential;

    public bool IsAuthenticated => _credential is not null;

    public TokenCredential GetCredential()
    {
        if (_credential is null)
        {
            throw new InvalidOperationException("Not authenticated. Call AuthenticateAsync first.");
        }
        return _credential;
    }

    public Task AuthenticateAsync(CancellationToken cancellationToken)
    {
        _credential = new InteractiveBrowserCredential();
        return Task.CompletedTask;
    }
}
