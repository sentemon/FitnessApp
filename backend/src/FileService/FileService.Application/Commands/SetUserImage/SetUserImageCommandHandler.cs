using FileService.Domain.Constants;
using FileService.Infrastructure.Interfaces;
using FileService.Persistence;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shared.Application.Abstractions;
using Shared.Application.Common;
using Shared.DTO.Messages;
using File = FileService.Domain.Entities.File;

namespace FileService.Application.Commands.SetUserImage;

public class SetUserImageCommandHandler : ICommandHandler<SetUserImageCommand, File>
{
    private readonly IAzureBlobStorageService _azureBlobStorageService;
    private readonly FileDbContext _context;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IConfiguration _configuration;
    private readonly ILogger<SetUserImageCommandHandler> _logger;

    public SetUserImageCommandHandler(IAzureBlobStorageService azureBlobStorageService, FileDbContext context, IPublishEndpoint publishEndpoint, IConfiguration configuration, ILogger<SetUserImageCommandHandler> logger)
    {
        _azureBlobStorageService = azureBlobStorageService;
        _context = context;
        _publishEndpoint = publishEndpoint;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<IResult<File, Error>> HandleAsync(SetUserImageCommand command)
    {
        if (command.UserId is null)
        {
            _logger.LogWarning("Attempted to set user image with a null UserId.");
            return Result<File>.Failure(new Error(ResponseMessages.UserIdIsNull));
        }
        
        if (command.FileStream is null)
        {
            _logger.LogWarning("Attempted to set user image with a null FileStream.");
            return Result<File>.Failure(new Error(ResponseMessages.FileStreamIsNull));
        }

        if (command.ContentType is null)
        {
            _logger.LogWarning("Attempted to set user image with a null ContentType.");
            return Result<File>.Failure(new Error(ResponseMessages.ContentTypeNull));
        }
        
        var blobName = await _azureBlobStorageService.UploadAsync(
            BlobContainerNamesConstants.UserAvatars,
            command.FileStream, 
            command.ContentType
        );
        
        var file = Domain.Entities.File.CreateFile(
            blobName, 
            BlobContainerNamesConstants.UserAvatars,
            command.FileStream.Length,
            command.UserId,
            Guid.Parse(command.UserId)
        );
        
        _context.Add(file);
        await _context.SaveChangesAsync();
        
        var host = _configuration[AppSettingsConstants.GatewayUrl];
        
        await _publishEndpoint.Publish(new UserImageSetEventMessage(
            file.ForeignEntityId.ToString(),
            $"{host}/file/files/users/{blobName}"
        ));

        _logger.LogInformation("User image set successfully for UserId: {UserId}. BlobName: {BlobName}", command.UserId, blobName);
        
        return Result<File>.Success(file);
    }
}