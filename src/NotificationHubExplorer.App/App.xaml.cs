using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using NotificationHubExplorer.App.ViewModels;
using NotificationHubExplorer.Infrastructure.DependencyInjection;
using NotificationHubExplorer.Infrastructure.NotificationHubs;

namespace NotificationHubExplorer.App;

public partial class App : Application
{
    private IHost? _host;
    private MainWindow? _mainWindow;

    public static IServiceProvider Services { get; private set; } = null!;

    public App()
    {
        InitializeComponent();
    }

    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                var settings = new NotificationHubSettings
                {
                    ConnectionString = context.Configuration["NotificationHubs:ConnectionString"]
                        ?? throw new InvalidOperationException(
                            "NotificationHubs:ConnectionString is required. " +
                            "Configure it in appsettings.json or as an environment variable.")
                };

                services.AddInfrastructure(settings);
                services.AddTransient<InstallationsViewModel>();
            })
            .Build();

        Services = _host.Services;
        await _host.StartAsync();

        _mainWindow = new MainWindow();
        _mainWindow.Activate();
    }
}
