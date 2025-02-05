using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using WorkoutService.Application.Commands.AddSet;
using WorkoutService.Application.Commands.CreateWorkout;
using WorkoutService.Application.Commands.DeleteWorkout;
using WorkoutService.Application.Commands.MarkSetAsCompleted;
using WorkoutService.Application.Commands.MarkSetAsUncompleted;
using WorkoutService.Application.Commands.UpdateWorkout;
using WorkoutService.Domain.Entities;
using WorkoutService.Domain.Enums;
using WorkoutService.Persistence;

namespace WorkoutService.Application.Tests;

public class TestFixture
{
    public readonly WorkoutDbContext WorkoutDbContextFixture;
    
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
        .WithImage("postgres:15-alpine")
        .Build();

    internal readonly CreateWorkoutCommandHandler CreateWorkoutCommandHandler;
    internal readonly UpdateWorkoutCommandHandler UpdateWorkoutCommandHandler;
    internal readonly DeleteWorkoutCommandHandler DeleteWorkoutCommandHandler;
    internal readonly AddSetCommandHandler AddSetCommandHandler;
    internal readonly MarkSetAsCompletedCommandHandler MarkSetAsCompletedCommandHandler;
    internal readonly MarkSetAsUncompletedCommandHandler MarkSetAsUncompletedCommandHandler;

    public User ExistingUser { get; }
    public Workout ExistingWorkout { get; }
    public Exercise ExistingExercise { get; }
    public Set ExistingSet { get; }

    public TestFixture()
    {
        _postgreSqlContainer.StartAsync().Wait();

        var connectionString = _postgreSqlContainer.GetConnectionString();
        
        var serviceProvider = TestStartup.Initialize(connectionString);

        WorkoutDbContextFixture = serviceProvider.GetRequiredService<WorkoutDbContext>();
        
        ApplyMigrations();
        
        CreateWorkoutCommandHandler = serviceProvider.GetRequiredService<CreateWorkoutCommandHandler>();
        UpdateWorkoutCommandHandler = serviceProvider.GetRequiredService<UpdateWorkoutCommandHandler>();
        DeleteWorkoutCommandHandler = serviceProvider.GetRequiredService<DeleteWorkoutCommandHandler>();
        AddSetCommandHandler = serviceProvider.GetRequiredService<AddSetCommandHandler>();
        MarkSetAsCompletedCommandHandler = serviceProvider.GetRequiredService<MarkSetAsCompletedCommandHandler>();
        MarkSetAsUncompletedCommandHandler = serviceProvider.GetRequiredService<MarkSetAsUncompletedCommandHandler>();
        
        ExistingUser = CreateExistingUser();
        ExistingWorkout = CreateExistingWorkout();
        ExistingExercise = CreateExistingExercise();
        // ExistingSet = CreateExistingSet();
    }

    private void ApplyMigrations()
    {
        using (var scope = WorkoutDbContextFixture.Database.BeginTransaction())
        {
            try
            {
                WorkoutDbContextFixture.Database.Migrate();
                scope.Commit();
            }
            catch (Exception)
            {
                scope.Rollback();
                throw;
            }
        }
    }

    private User CreateExistingUser()
    {
        var user = User.Create(
            Guid.NewGuid().ToString(),
            "First Name",
            "Last Name",
            "Username",
            "https://example.com/image"
        );
        
        WorkoutDbContextFixture.Users.Add(user);
        WorkoutDbContextFixture.SaveChanges();

        return user;
    }
    
    private Workout CreateExistingWorkout()
    {
        var workout = Workout.Create(
            "Example Title",
            "Example Description",
            35u,
            DifficultyLevel.Intermediate,
            ExistingUser.Id
        );
        
        workout.SetImageUrl("https://example.com");
        
        WorkoutDbContextFixture.Workouts.Add(workout);
        WorkoutDbContextFixture.SaveChanges();

        return workout;
    }
    
    private Exercise CreateExistingExercise()
    {
        var exercise = Exercise.Create(
            "Example Name",
            DifficultyLevel.AllLevels,
            ExistingUser.Id
        );

        WorkoutDbContextFixture.Exercises.Add(exercise);
        WorkoutDbContextFixture.SaveChanges();

        return exercise;
    }
    
    private Set CreateExistingSet()
    {
        var set = Set.Create(
            12u,
            20,
            ExistingExercise.Id
        );

        WorkoutDbContextFixture.Sets.Add(set);
        WorkoutDbContextFixture.SaveChanges();

        return set;
    }
}
