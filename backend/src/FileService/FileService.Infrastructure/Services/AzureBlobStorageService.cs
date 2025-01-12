using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FileService.Infrastructure.Interfaces;
using BlobInfo = FileService.Infrastructure.Models.BlobInfo;

namespace FileService.Infrastructure.Services;

public class AzureBlobStorageService : IAzureBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;

    public AzureBlobStorageService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<BlobInfo> DownloadAsync(string blobName, string containerName)
    {
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = blobContainerClient.GetBlobClient(blobName);

        var blobDownloadInfo = await blobClient.DownloadAsync();

        return new BlobInfo(blobDownloadInfo.Value.Content, blobDownloadInfo.Value.ContentType);
    }

    public async Task<string> UploadAsync(string containerName, Stream stream, string contentType)
    {
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

        var blobName = Guid.NewGuid().ToString();
        var blobClient = blobContainerClient.GetBlobClient(blobName);
        
        await blobClient.UploadAsync(stream, new BlobHttpHeaders
        {
            ContentType = contentType
        });

        return blobName;
    }

    public async Task DeleteAsync(string blobName, string containerName)
    {
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = blobContainerClient.GetBlobClient(blobName);

        await blobClient.DeleteIfExistsAsync();
    }
}