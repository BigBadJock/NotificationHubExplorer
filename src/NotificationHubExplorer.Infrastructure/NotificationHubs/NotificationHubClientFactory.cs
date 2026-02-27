using System.Collections.Concurrent;
using Microsoft.Azure.NotificationHubs;

namespace NotificationHubExplorer.Infrastructure.NotificationHubs;

public sealed class NotificationHubClientFactory
{
    private readonly NotificationHubSettings _settings;
    private readonly ConcurrentDictionary<string, NotificationHubClient> _clients = new();

    public NotificationHubClientFactory(NotificationHubSettings settings)
    {
        _settings = settings;
    }

    public NotificationHubClient GetClient(string hubName)
    {
        return _clients.GetOrAdd(hubName, name =>
            NotificationHubClient.CreateClientFromConnectionString(
                _settings.ConnectionString,
                name));
    }
}
