using FileService.Infrastructure.Models;

namespace FileService.Infrastructure.Interfaces;

public interface IAzureBlobStorageService
{
    Task<BlobInfo> DownloadAsync(Guid id, string containerName);
    Task<string> UploadAsync(string containerName, Stream stream, string contentType);
    Task DeleteAsync(Guid id, string containerName);
}