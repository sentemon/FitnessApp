using FileService.Domain.Constants;
using FileService.Infrastructure.Interfaces;
using FileService.Infrastructure.Models;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace FileService.Application.Queries.DownloadPost;

public class DownloadPostQueryHandler : IQueryHandler<DownloadPostQuery, BlobInfo>
{
    private readonly IAzureBlobStorageService _azureBlobStorageService;

    public DownloadPostQueryHandler(IAzureBlobStorageService azureBlobStorageService)
    {
        _azureBlobStorageService = azureBlobStorageService;
    }

    public async Task<IResult<BlobInfo, Error>> HandleAsync(DownloadPostQuery query)
    {
        var blobInfo = await _azureBlobStorageService.DownloadAsync(query.BlobName, BlobContainerNamesConstants.PostPhotos);

        return Result<BlobInfo>.Success(blobInfo);
    }
}