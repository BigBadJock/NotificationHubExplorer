namespace NotificationHubExplorer.Core.Models;

public sealed class NamespaceInfo
{
    public required string Name { get; init; }
    public required string SubscriptionId { get; init; }
    public required string ResourceGroupName { get; init; }
    public required string Location { get; init; }
}
