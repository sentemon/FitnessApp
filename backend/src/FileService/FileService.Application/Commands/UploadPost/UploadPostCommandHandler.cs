using FileService.Domain.Constants;
using FileService.Infrastructure.Interfaces;
using FileService.Persistence;
using MassTransit;
using Microsoft.AspNetCore.Http;
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
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UploadPostCommandHandler(IAzureBlobStorageService azureBlobStorageService, FileDbContext context, IPublishEndpoint publishEndpoint, IHttpContextAccessor httpContextAccessor)
    {
        _azureBlobStorageService = azureBlobStorageService;
        _context = context;
        _publishEndpoint = publishEndpoint;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IResult<File, Error>> HandleAsync(UploadPostCommand command)
    {
        if (command.UserId == null)
        {
            return Result<File>.Failure(new Error(ResponseMessages.UserIdIsNull));
        }

        if (command.UploadPostFileDto.FileStream == null)
        {
            return Result<File>.Failure(new Error(ResponseMessages.FileStreamIsNull));
        }
        
        if (command.UploadPostFileDto.ContentType == null)
        {
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

        var request = _httpContextAccessor.HttpContext?.Request;
        var host = $"{request?.Scheme}://{request?.Host}";
        
        await _publishEndpoint.Publish(new PostUploadedEventMessage(
            file.ForeignEntityId,
            $"{host}/file/files/{blobName}"
        ));

        return Result<File>.Success(file);
    }
}