using FileService.Domain.Constants;
using FileService.Infrastructure.Interfaces;
using FileService.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shared.Application.Common;
using Shared.Application.Abstractions;
using Shared.DTO.Messages;
using File = FileService.Domain.Entities.File;

namespace FileService.Application.Commands.UploadPost;

public class UploadPostCommandHandler : ICommandHandler<UploadPostCommand, PostUploadedEventMessage>
{
    private readonly IFileService _fileService;
    private readonly FileDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<UploadPostCommandHandler> _logger;

    public UploadPostCommandHandler(IFileService fileService, FileDbContext context, IConfiguration configuration, ILogger<UploadPostCommandHandler> logger)
    {
        _fileService = fileService;
        _context = context;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<IResult<PostUploadedEventMessage, Error>> HandleAsync(UploadPostCommand command)
    {
        if (command.UserId == null)
        {
            _logger.LogWarning("Attempted to upload a post file with a null UserId.");
            return Result<PostUploadedEventMessage>.Failure(new Error(ResponseMessages.UserIdIsNull));
        }

        if (command.UploadPostFileDto.FileStream == null)
        {
            _logger.LogWarning("Attempted to upload a post file with a null FileStream.");
            return Result<PostUploadedEventMessage>.Failure(new Error(ResponseMessages.FileStreamIsNull));
        }
        
        if (command.UploadPostFileDto.ContentType == null)
        {
            _logger.LogWarning("Attempted to upload a post file with a null ContentType.");
            return Result<PostUploadedEventMessage>.Failure(new Error(ResponseMessages.ContentTypeNull));
        }
        
        var blobName = await _fileService.UploadAsync(
            BlobContainerNamesConstants.PostPhotos,
            command.UploadPostFileDto.FileStream, 
            command.UploadPostFileDto.ContentType
        );

        var file = File.CreateFile(
            blobName, 
            BlobContainerNamesConstants.PostPhotos,
            command.UploadPostFileDto.FileStream.Length, 
            command.UserId,
            command.UploadPostFileDto.PostId
        );

        _context.Add(file);
        await _context.SaveChangesAsync();
        
        var host = _configuration[AppSettingsConstants.GatewayUrl];
        
        var postUploadedEventMessage = new PostUploadedEventMessage(
            file.ForeignEntityId,
            $"{host}/file/files/posts/{blobName}"
        );
        
        _logger.LogInformation("Post file uploaded successfully. FileId: {FileId}, BlobName: {BlobName}", file.Id, blobName);

        return Result<PostUploadedEventMessage>.Success(postUploadedEventMessage);
    }
}