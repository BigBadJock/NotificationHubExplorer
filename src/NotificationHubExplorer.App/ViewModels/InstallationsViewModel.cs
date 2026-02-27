using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotificationHubExplorer.Core.Interfaces;
using NotificationHubExplorer.Core.Models;

namespace NotificationHubExplorer.App.ViewModels;

public sealed partial class InstallationsViewModel : ObservableObject
{
    private readonly INotificationHubService _notificationHubService;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotLoading))]
    private bool _isLoading;

    public bool IsNotLoading => !IsLoading;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private string _selectedHubName = string.Empty;

    public ObservableCollection<InstallationSummary> Installations { get; } = new();

    public InstallationsViewModel(INotificationHubService notificationHubService)
    {
        _notificationHubService = notificationHubService;
    }

    [RelayCommand(IncludeCancelCommand = true)]
    private async Task LoadInstallationsAsync(CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(SelectedHubName))
        {
            StatusMessage = "Please select a hub.";
            return;
        }

        IsLoading = true;
        StatusMessage = string.Empty;
        Installations.Clear();

        try
        {
            string? continuationToken = null;
            do
            {
                var result = await _notificationHubService.GetInstallationsAsync(
                    SelectedHubName,
                    continuationToken,
                    cancellationToken);

                foreach (var item in result.Items)
                {
                    Installations.Add(item);
                }

                continuationToken = result.ContinuationToken;
            }
            while (continuationToken is not null);

            StatusMessage = $"Loaded {Installations.Count} installation(s).";
        }
        catch (OperationCanceledException)
        {
            StatusMessage = "Load cancelled.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
}
