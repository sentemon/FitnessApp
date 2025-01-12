using HotChocolate.Types;
using Microsoft.AspNetCore.StaticFiles;

namespace PostService.Application.Extensions;

public static class FileExtension
{
    public static string GetContentType(IFile? file)
    {
        if (file == null)
        {
            return "text/plain";
        }
        
        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(file.Name, out var contentType))
        {
            return "application/octet-stream";
        }
        
        return contentType;
    }
}