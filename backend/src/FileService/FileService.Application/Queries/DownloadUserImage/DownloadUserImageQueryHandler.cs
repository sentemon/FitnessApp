using FileService.Domain.Constants;
using FileService.Infrastructure.Interfaces;
using FileService.Infrastructure.Models;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace FileService.Application.Queries.DownloadUserImage;

public class DownloadUserImageQueryHandler : IQueryHandler<DownloadUserImageQuery, BlobInfo>
{
    private readonly IFileService _fileService;

    public DownloadUserImageQueryHandler(IFileService fileService)
    {
        _fileService = fileService;
    }

    public async Task<IResult<BlobInfo, Error>> HandleAsync(DownloadUserImageQuery query)
    {
            var blobInfo = await _fileService.DownloadAsync(query.BlobName, BlobContainerNamesConstants.UserAvatars);

            return Result<BlobInfo>.Success(blobInfo);
    }
}