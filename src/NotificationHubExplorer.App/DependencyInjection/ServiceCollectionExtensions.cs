using Microsoft.Extensions.DependencyInjection;
using NotificationHubExplorer.App.ViewModels;

namespace NotificationHubExplorer.App.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppViewModels(this IServiceCollection services)
    {
        services.AddTransient<InstallationsViewModel>();
        return services;
    }
}
