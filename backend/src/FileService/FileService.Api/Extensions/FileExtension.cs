using Microsoft.AspNetCore.StaticFiles;

namespace FileService.Api.Extensions;

public static class FileExtension
{
    private static readonly FileExtensionContentTypeProvider Provider = new();

    public static string GetContentType(this string fileName)
    {
        if (!Provider.TryGetContentType(fileName, out var contentType))
        {
            contentType = "application/octet-stream";
        }

        return contentType;
    }
}