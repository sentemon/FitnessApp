using Azure.Storage.Blobs;
using FileService.Domain.Constants;
using FileService.Infrastructure.Interfaces;
using FileService.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var azureStorageConnectionString = configuration.GetConnectionString(AppSettingsConstants.AzureStorageConnectionString);
        
        services.AddSingleton(new BlobServiceClient(azureStorageConnectionString));
        services.AddSingleton<IFileService, AzureBlobStorageService>();
        
        return services;
    }
}