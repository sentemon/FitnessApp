using Shared.Application.Abstractions;

namespace FileService.Application.Commands.DownloadPost;

public record DownloadPostCommand(string BlobName) : ICommand;