using Gateway.Api.Constants;

var builder = WebApplication.CreateBuilder(args);

var allowedOrigins = builder.Configuration.GetSection(AppSettingsConstants.AllowedOrigins).Get<string[]>();
var hostingUrl = builder.Configuration[AppSettingsConstants.WebHostUrl];

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

var app = builder.Build();

app.UseCors(CorsConstants.CorsPolicy);

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.Run();