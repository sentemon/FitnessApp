namespace AuthService.Infrastructure.DelegatingHandlers;

public class ExceptionHandlingHttpClientHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);

            throw new HttpRequestException($"HTTP {(int)response.StatusCode}: {response.ReasonPhrase}. Content: {errorContent}");
        }

        return response;
    }
}