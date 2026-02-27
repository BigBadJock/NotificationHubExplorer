namespace NotificationHubExplorer.Core.Models;

public sealed class HubInfo
{
    public required string Name { get; init; }
    public required string NamespaceName { get; init; }
    public required string SubscriptionId { get; init; }
    public required string ResourceGroupName { get; init; }
}
