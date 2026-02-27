namespace NotificationHubExplorer.Core.Models;

public sealed class InstallationDetails
{
    public required string InstallationId { get; init; }
    public required string Platform { get; init; }
    public string? PushChannel { get; init; }
    public DateTimeOffset? ExpirationTime { get; init; }
    public IReadOnlyDictionary<string, string> Tags { get; init; } = new Dictionary<string, string>();
    public IReadOnlyDictionary<string, string> Templates { get; init; } = new Dictionary<string, string>();
    public DateTimeOffset? LastUpdated { get; init; }
}
