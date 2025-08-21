using Shared.Application.Abstractions;

namespace FileService.Application.Queries.DownloadUserImage;

public record DownloadUserImageQuery(string BlobName) : IQuery;