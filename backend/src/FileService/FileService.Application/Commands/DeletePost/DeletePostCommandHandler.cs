using FileService.Domain.Constants;
using FileService.Infrastructure.Interfaces;
using FileService.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions;
using Shared.Application.Common;

namespace FileService.Application.Commands.DeletePost;

public class DeletePostCommandHandler : ICommandHandler<DeletePostCommand, string>
{
    private readonly IFileService _fileService;
    private readonly FileDbContext _context;
    private readonly ILogger<DeletePostCommandHandler> _logger;

    public DeletePostCommandHandler(IFileService fileService, FileDbContext context, ILogger<DeletePostCommandHandler> logger)
    {
        _fileService = fileService;
        _context = context;
        _logger = logger;
    }

    public async Task<IResult<string, Error>> HandleAsync(DeletePostCommand command)
    {
        var file = await _context.Files.FirstOrDefaultAsync(f => f.ForeignEntityId == command.PostId);

        if (file == null)
        {
            _logger.LogWarning("Attempted to delete a file for a post that does not exist: PostId: {PostId}", command.PostId);
            return Result<string>.Failure(new Error(ResponseMessages.FileNotFound));
        }
        
        await _fileService.DeleteAsync(file.BlobName, file.BlobContainerName);
        
        _context.Remove(file);
        await _context.SaveChangesAsync();
        
        return Result<string>.Success(ResponseMessages.YouSuccessfullyDeletedFile);
    }
}