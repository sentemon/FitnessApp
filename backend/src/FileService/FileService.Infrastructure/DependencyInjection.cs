using Azure.Storage.Blobs;
using FileService.Domain.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var azureStorageConnectionString = configuration.GetConnectionString(AppSettingsConstants.AzureStorageConnectionString);
        
        services.AddSingleton(new BlobServiceClient(azureStorageConnectionString));
        
        return services;
    }
}