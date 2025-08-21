using FileService.Domain.Constants;
using FileService.Infrastructure.Interfaces;
using FileService.Persistence;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shared.Application.Common;
using Shared.Application.Abstractions;
using Shared.DTO.Messages;
using File = FileService.Domain.Entities.File;

namespace FileService.Application.Commands.UploadPost;

public class UploadPostCommandHandler : ICommandHandler<UploadPostCommand, File>
{
    private readonly IAzureBlobStorageService _azureBlobStorageService;
    private readonly FileDbContext _context;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IConfiguration _configuration;
    private readonly ILogger<UploadPostCommandHandler> _logger;

    public UploadPostCommandHandler(IAzureBlobStorageService azureBlobStorageService, FileDbContext context, IPublishEndpoint publishEndpoint, IConfiguration configuration, ILogger<UploadPostCommandHandler> logger)
    {
        _azureBlobStorageService = azureBlobStorageService;
        _context = context;
        _publishEndpoint = publishEndpoint;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<IResult<File, Error>> HandleAsync(UploadPostCommand command)
    {
        if (command.UserId == null)
        {
            _logger.LogWarning("Attempted to upload a post file with a null UserId.");
            return Result<File>.Failure(new Error(ResponseMessages.UserIdIsNull));
        }

        if (command.UploadPostFileDto.FileStream == null)
        {
            _logger.LogWarning("Attempted to upload a post file with a null FileStream.");
            return Result<File>.Failure(new Error(ResponseMessages.FileStreamIsNull));
        }
        
        if (command.UploadPostFileDto.ContentType == null)
        {
            _logger.LogWarning("Attempted to upload a post file with a null ContentType.");
            return Result<File>.Failure(new Error(ResponseMessages.ContentTypeNull));
        }
        
        var blobName = await _azureBlobStorageService.UploadAsync(
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
        
        await _publishEndpoint.Publish(new PostUploadedEventMessage(
            file.ForeignEntityId,
            $"{host}/file/files/posts/{blobName}"
        ));
        
        _logger.LogInformation("Post file uploaded successfully. FileId: {FileId}, BlobName: {BlobName}", file.Id, blobName);

        return Result<File>.Success(file);
    }
}