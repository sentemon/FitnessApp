using Azure.Storage.Blobs;
using FileService.Domain.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(new BlobServiceClient(configuration.GetConnectionString(AppSettingsConstants.AzureStorageConnectionString)));
        
        return services;
    }
}