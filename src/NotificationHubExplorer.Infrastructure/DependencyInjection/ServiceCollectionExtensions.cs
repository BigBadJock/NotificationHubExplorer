using Microsoft.Extensions.DependencyInjection;
using NotificationHubExplorer.Core.Interfaces;
using NotificationHubExplorer.Infrastructure.Authentication;
using NotificationHubExplorer.Infrastructure.NotificationHubs;
using NotificationHubExplorer.Infrastructure.ResourceDiscovery;

namespace NotificationHubExplorer.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, NotificationHubSettings settings)
    {
        services.AddSingleton<AzureAuthenticationService>();
        services.AddSingleton<IAzureAuthenticationService>(sp => sp.GetRequiredService<AzureAuthenticationService>());
        services.AddSingleton<IAzureResourceDiscoveryService, AzureResourceDiscoveryService>();
        services.AddSingleton(settings);
        services.AddSingleton<NotificationHubClientFactory>();
        services.AddSingleton<INotificationHubService, NotificationHubService>();
        return services;
    }
}
