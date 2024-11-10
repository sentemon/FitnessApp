using Xunit;

namespace PostService.Application.Tests;

public class TestBase : IClassFixture<TestFixture>, IAsyncLifetime
{
    protected readonly TestFixture Fixture;

    public TestBase(TestFixture fixture)
    {
        Fixture = fixture;
    }

    public async Task InitializeAsync()
    {
        await Fixture.PostDbContextFixture.Database.EnsureDeletedAsync();
        await Fixture.PostDbContextFixture.Database.EnsureCreatedAsync();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}