using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using NotificationHubExplorer.App.ViewModels;

namespace NotificationHubExplorer.App.Views;

public sealed partial class InstallationsPage : Page
{
    public InstallationsViewModel ViewModel { get; }

    public InstallationsPage()
    {
        ViewModel = App.Services.GetRequiredService<InstallationsViewModel>();
        InitializeComponent();
    }
}
