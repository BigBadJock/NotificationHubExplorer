using Azure.ResourceManager;
using Azure.ResourceManager.NotificationHubs;
using NotificationHubExplorer.Core.Interfaces;
using NotificationHubExplorer.Core.Models;
using NotificationHubExplorer.Infrastructure.Authentication;

namespace NotificationHubExplorer.Infrastructure.ResourceDiscovery;

public sealed class AzureResourceDiscoveryService : IAzureResourceDiscoveryService
{
    private readonly AzureAuthenticationService _authService;

    public AzureResourceDiscoveryService(AzureAuthenticationService authService)
    {
        _authService = authService;
    }

    public async Task<IEnumerable<SubscriptionInfo>> GetSubscriptionsAsync(CancellationToken cancellationToken)
    {
        var credential = _authService.GetCredential();
        var armClient = new ArmClient(credential);
        var subscriptions = new List<SubscriptionInfo>();

        await foreach (var subscription in armClient.GetSubscriptions().GetAllAsync(cancellationToken))
        {
            subscriptions.Add(new SubscriptionInfo
            {
                SubscriptionId = subscription.Data.SubscriptionId,
                DisplayName = subscription.Data.DisplayName
            });
        }

        return subscriptions;
    }

    public async Task<IEnumerable<NamespaceInfo>> GetNamespacesAsync(string subscriptionId, CancellationToken cancellationToken)
    {
        var credential = _authService.GetCredential();
        var armClient = new ArmClient(credential);
        var subscription = await armClient.GetSubscriptions().GetAsync(subscriptionId, cancellationToken);
        var namespaces = new List<NamespaceInfo>();

        await foreach (var ns in subscription.Value.GetNotificationHubNamespacesAsync(cancellationToken: cancellationToken))
        {
            namespaces.Add(new NamespaceInfo
            {
                Name = ns.Data.Name,
                SubscriptionId = subscriptionId,
                ResourceGroupName = ns.Id.ResourceGroupName!,
                Location = ns.Data.Location.ToString()
            });
        }

        return namespaces;
    }

    public async Task<IEnumerable<HubInfo>> GetHubsAsync(string subscriptionId, string namespaceName, CancellationToken cancellationToken)
    {
        var credential = _authService.GetCredential();
        var armClient = new ArmClient(credential);
        var subscription = await armClient.GetSubscriptions().GetAsync(subscriptionId, cancellationToken);
        var hubs = new List<HubInfo>();

        await foreach (var resourceGroup in subscription.Value.GetResourceGroups().GetAllAsync(cancellationToken: cancellationToken))
        {
            if (!resourceGroup.GetNotificationHubNamespaces().Exists(namespaceName))
            {
                continue;
            }

            var ns = await resourceGroup.GetNotificationHubNamespaces().GetAsync(namespaceName, cancellationToken);

            await foreach (var hub in ns.Value.GetNotificationHubs().GetAllAsync(cancellationToken: cancellationToken))
            {
                hubs.Add(new HubInfo
                {
                    Name = hub.Data.Name,
                    NamespaceName = namespaceName,
                    SubscriptionId = subscriptionId,
                    ResourceGroupName = resourceGroup.Data.Name
                });
            }
        }

        return hubs;
    }
}
