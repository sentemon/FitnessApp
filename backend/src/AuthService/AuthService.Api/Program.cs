var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://0.0.0.0:8082");

var app = builder.Build();

app.UseHttpsRedirection();

app.Run();