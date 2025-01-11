using FileService.Domain.Constants;
using FileService.Infrastructure.Interfaces;
using FileService.Infrastructure.Models;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace FileService.Application.Commands.DownloadPost;

public class DownloadPostCommandHandler : ICommandHandler<DownloadPostCommand, BlobInfo>
{
    private readonly IAzureBlobStorageService _azureBlobStorageService;

    public DownloadPostCommandHandler(IAzureBlobStorageService azureBlobStorageService)
    {
        _azureBlobStorageService = azureBlobStorageService;
    }

    public async Task<IResult<BlobInfo, Error>> HandleAsync(DownloadPostCommand command)
    {
        var blobInfo = await _azureBlobStorageService.DownloadAsync(command.Id, BlobContainerNamesConstants.PostPhotos);

        return Result<BlobInfo>.Success(blobInfo);
    }
}