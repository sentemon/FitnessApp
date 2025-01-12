using Shared.Application.Abstractions;

namespace FileService.Application.Queries.DownloadPost;

public record DownloadPostQuery(string BlobName) : IQuery;