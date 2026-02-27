using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using NotificationHubExplorer.App.Views;

namespace NotificationHubExplorer.App;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        ContentFrame.Navigate(typeof(InstallationsPage));
    }

    private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.SelectedItem is NavigationViewItem item)
        {
            switch (item.Tag?.ToString())
            {
                case "installations":
                    ContentFrame.Navigate(typeof(InstallationsPage));
                    break;
            }
        }
    }
}
