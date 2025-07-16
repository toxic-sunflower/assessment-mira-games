using Microsoft.Extensions.DependencyInjection;

namespace LocalizationService.Management.Client;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddManagementClient(
        this IServiceCollection services,
        string apiBaseAddress)
    {
        services.Configure<ManagementClientOptions>(options => options.BaseUrl = apiBaseAddress);
        services.AddHttpClient<ManagementClient>();
        services.AddSingleton<IManagementClient, ManagementClient>();
        services.AddSingleton<ILanguageClient>(svc => svc.GetRequiredService<IManagementClient>());
        
        return services;
    }
}