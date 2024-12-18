namespace AuthService.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (HttpRequestException ex)
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsync($"HTTP Error: {ex.Message}");
        }
        catch (TaskCanceledException ex)
        {
            httpContext.Response.StatusCode = StatusCodes.Status408RequestTimeout;
            await httpContext.Response.WriteAsync($"Request Timeout: {ex.Message}");
        }
        catch (Exception ex)
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsync($"Unexpected error: {ex.Message}");
        }
    }
}