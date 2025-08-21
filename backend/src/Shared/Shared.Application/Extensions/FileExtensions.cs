using HotChocolate.Types;
using Microsoft.AspNetCore.StaticFiles;

namespace Shared.Application.Extensions;

public static class FileExtensions
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
    
    public static byte[] ReadFully(Stream? input)
    { 
        using var ms = new MemoryStream();
        input?.CopyTo(ms);
        
        return ms.ToArray();
    }
}