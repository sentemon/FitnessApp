using FileService.Application;
using FileService.Application.Commands.DownloadPost;
using FileService.Domain.Constants;
using FileService.Infrastructure;
using FileService.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var hostingUrl = builder.Configuration[AppSettingsConstants.WebHostUrl];

builder.WebHost.UseUrls(hostingUrl ?? throw new ArgumentNullException(nameof(hostingUrl), "Hosting URL is not configured."));

builder.Services
    .AddPersistenceServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration)
    .AddApplicationServices(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<FileDbContext>();
    dbContext.Database.Migrate();
}

app.MapGet("/health", () => Results.Ok("Healthy"));

app.MapGet("/files/{blobName}", async (string blobName, DownloadPostCommandHandler downloadPostCommandHandler) =>
{
    var command = new DownloadPostCommand(blobName);

    var result = await downloadPostCommandHandler.HandleAsync(command);

    if (!result.IsSuccess)
    {
        return Results.NotFound(result.Error.Message);
    }

    return Results.File(result.Response.Content, result.Response.ContentType);
});

app.Run();