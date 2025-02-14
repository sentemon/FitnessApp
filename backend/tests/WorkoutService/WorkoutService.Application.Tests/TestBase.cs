using Xunit;

namespace WorkoutService.Application.Tests;

public class TestBase : IClassFixture<TestFixture>, IAsyncLifetime
{
    protected readonly TestFixture Fixture;

    protected TestBase(TestFixture fixture)
    {
        Fixture = fixture;
    }

    public async Task InitializeAsync()
    {
        await Fixture.WorkoutDbContextFixture.Database.EnsureCreatedAsync();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}