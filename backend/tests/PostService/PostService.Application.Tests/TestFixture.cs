using Microsoft.Extensions.DependencyInjection;
using PostService.Application.Commands.AddPost;
using PostService.Application.Commands.DeletePost;
using PostService.Persistence;
using Testcontainers.PostgreSql;

namespace PostService.Application.Tests;

public class TestFixture
{
    private readonly TestStartup _testStartup = new();
    public readonly PostDbContext PostDbContextFixture;
    
    public readonly AddPostCommandHandler AddPostCommandHandler;
    public readonly DeletePostCommandHandler DeletePostCommandHandler;
    
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
        .WithImage("postgres:15-alpine")
        .Build();

    public TestFixture()
    {
        _postgreSqlContainer.StartAsync().Wait();

        var connectionString = _postgreSqlContainer.GetConnectionString();
        var serviceProvider = _testStartup.Initialize(connectionString);

        PostDbContextFixture = serviceProvider.GetRequiredService<PostDbContext>();

        AddPostCommandHandler = serviceProvider.GetRequiredService<AddPostCommandHandler>();
        DeletePostCommandHandler = serviceProvider.GetRequiredService<DeletePostCommandHandler>();
    }
}
