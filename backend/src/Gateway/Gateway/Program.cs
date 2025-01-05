using Gateway.Constants;

var builder = WebApplication.CreateBuilder(args);

var allowedOrigins = builder.Configuration.GetSection(AppSettingsConstants.AllowedOrigins).Get<string[]>();
var hostingUrl = builder.Configuration[AppSettingsConstants.WebHostUrl];
var reverseProxyConfig = builder.Configuration.GetSection(AppSettingsConstants.ReverseProxy);

builder.WebHost.UseUrls(hostingUrl ?? throw new ArgumentNullException(nameof(hostingUrl), "Hosting URL is not configured."));

builder.Services
    .AddCors(options =>
    {
        options.AddPolicy(CorsConstants.CorsPolicy, policyBuilder =>
        {
            policyBuilder
                .WithOrigins(allowedOrigins ?? throw new ArgumentNullException(nameof(allowedOrigins),
                    "Allowed Origin URLs are not configured."))
                .AllowCredentials()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(reverseProxyConfig);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors(CorsConstants.CorsPolicy);

app.MapReverseProxy();

app.MapGet("/health" ,() => Results.Ok("Healthy"));

app.Run();