using FileService.Infrastructure.Models;

namespace FileService.Infrastructure.Interfaces;

public interface IFileService
{
    Task<BlobInfo> DownloadAsync(string blobName, string containerName);
    Task<string> UploadAsync(string containerName, Stream stream, string contentType);
    Task DeleteAsync(string blobName, string containerName);
}