namespace NotificationHubExplorer.Core.Models;

public sealed class InstallationSummary
{
    public required string InstallationId { get; init; }
    public required string Platform { get; init; }
    public string? PushChannel { get; init; }
    public DateTimeOffset? ExpirationTime { get; init; }
}
