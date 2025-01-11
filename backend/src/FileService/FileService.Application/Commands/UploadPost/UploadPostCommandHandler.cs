using FileService.Domain.Constants;
using FileService.Infrastructure.Interfaces;
using FileService.Persistence;
using MassTransit;
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

    public UploadPostCommandHandler(IAzureBlobStorageService azureBlobStorageService, FileDbContext context, IPublishEndpoint publishEndpoint)
    {
        _azureBlobStorageService = azureBlobStorageService;
        _context = context;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<IResult<File, Error>> HandleAsync(UploadPostCommand command)
    {
        if (command.UserId == null)
        {
            return Result<File>.Failure(new Error("User Id is null"));
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

        await _publishEndpoint.Publish(new PostUploadedEventMessage(
            file.ForeignEntityId,
            "http://localhost:8000/file/{id}" // ToDo
        ));

        return Result<File>.Success(file);
    }
}