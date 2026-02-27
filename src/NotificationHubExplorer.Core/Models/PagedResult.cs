namespace NotificationHubExplorer.Core.Models;

public sealed class PagedResult<T>
{
    public required IReadOnlyList<T> Items { get; init; }
    public string? ContinuationToken { get; init; }
    public bool HasMore => ContinuationToken is not null;

    public static PagedResult<T> Empty() =>
        new() { Items = Array.Empty<T>(), ContinuationToken = null };
}
