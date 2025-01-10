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

    public async Task<BlobInfo> DownloadAsync(Guid id, string containerName)
    {
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = blobContainerClient.GetBlobClient(id.ToString());

        var blobDownloadInfo = await blobClient.DownloadAsync();

        return new BlobInfo(blobDownloadInfo.Value.Content, blobDownloadInfo.Value.ContentType);
    }

    public async Task<Guid> UploadAsync(string containerName, Stream stream, string contentType)
    {
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

        var blobName = Guid.NewGuid();
        var blobClient = blobContainerClient.GetBlobClient(blobName.ToString());
        
        await blobClient.UploadAsync(stream, new BlobHttpHeaders
        {
            ContentType = contentType
        });

        return blobName;
    }

    public async Task DeleteAsync(Guid id, string containerName)
    {
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = blobContainerClient.GetBlobClient(id.ToString());

        await blobClient.DeleteIfExistsAsync();
    }
}