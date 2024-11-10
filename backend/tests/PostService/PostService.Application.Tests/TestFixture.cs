using PostService.Persistence;
using Testcontainers.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace PostService.Application.Tests;

public class TestFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer;
    public readonly PostDbContext PostDbContextFixture;

    public TestFixture()
    {
        _postgreSqlContainer = new PostgreSqlBuilder()
            .WithImage("postgres:15-alpine")
            .WithDatabase("TestDb")
            .WithUsername("testuser")
            .WithPassword("password")
            .WithCleanUp(true)
            .Build();
        
        var options = new DbContextOptionsBuilder<PostDbContext>()
            .UseNpgsql(_postgreSqlContainer.GetConnectionString())
            .Options;

        PostDbContextFixture = new PostDbContext(options);
    }

    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();
        await PostDbContextFixture.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await PostDbContextFixture.DisposeAsync();
        await _postgreSqlContainer.StopAsync();
    }
}