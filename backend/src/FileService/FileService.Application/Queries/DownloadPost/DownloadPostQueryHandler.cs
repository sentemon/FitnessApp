using FileService.Domain.Constants;
using FileService.Infrastructure.Interfaces;
using FileService.Infrastructure.Models;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace FileService.Application.Queries.DownloadPost;

public class DownloadPostQueryHandler : IQueryHandler<DownloadPostQuery, BlobInfo>
{
    private readonly IFileService _fileService;

    public DownloadPostQueryHandler(IFileService fileService)
    {
        _fileService = fileService;
    }

    public async Task<IResult<BlobInfo, Error>> HandleAsync(DownloadPostQuery query)
    {
        var blobInfo = await _fileService.DownloadAsync(query.BlobName, BlobContainerNamesConstants.PostPhotos);

        return Result<BlobInfo>.Success(blobInfo);
    }
}