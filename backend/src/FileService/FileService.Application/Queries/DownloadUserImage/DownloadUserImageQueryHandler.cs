using FileService.Domain.Constants;
using FileService.Infrastructure.Interfaces;
using FileService.Infrastructure.Models;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace FileService.Application.Queries.DownloadUserImage;

public class DownloadUserImageQueryHandler : IQueryHandler<DownloadUserImageQuery, BlobInfo>
{
    private readonly IAzureBlobStorageService _azureBlobStorageService;

    public DownloadUserImageQueryHandler(IAzureBlobStorageService azureBlobStorageService)
    {
        _azureBlobStorageService = azureBlobStorageService;
    }

    public async Task<IResult<BlobInfo, Error>> HandleAsync(DownloadUserImageQuery query)
    {
            var blobInfo = await _azureBlobStorageService.DownloadAsync(query.BlobName, BlobContainerNamesConstants.UserAvatars);

            return Result<BlobInfo>.Success(blobInfo);
    }
}