namespace AuthService.Application.Tests;

public class TestBase : IClassFixture<TestFixture>, IAsyncLifetime
{
    protected readonly TestFixture Fixture;

    protected TestBase(TestFixture fixture)
    {
        Fixture = fixture;
    }
    
    public async Task InitializeAsync()
    {
        await Fixture.AuthDbContextFixture.Database.EnsureCreatedAsync();
    }

    public Task DisposeAsync()
    { 
        return Task.CompletedTask;
    }
}