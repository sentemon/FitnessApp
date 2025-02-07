using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using WorkoutService.Application.Commands.AddSet;
using WorkoutService.Application.Commands.AddSetHistory;
using WorkoutService.Application.Commands.CreateWorkout;
using WorkoutService.Application.Commands.DeleteSet;
using WorkoutService.Application.Commands.DeleteWorkout;
using WorkoutService.Application.Commands.MarkSetAsCompleted;
using WorkoutService.Application.Commands.MarkSetAsUncompleted;
using WorkoutService.Application.Commands.SetUpProfile;
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
    internal readonly DeleteSetCommandHandler DeleteSetCommandHandler;
    internal readonly AddSetHistoryCommandHandler AddSetHistoryCommandHandler;
    internal readonly MarkSetAsCompletedCommandHandler MarkSetAsCompletedCommandHandler;
    internal readonly MarkSetAsUncompletedCommandHandler MarkSetAsUncompletedCommandHandler;
    internal readonly SetUpProfileCommandHandler SetUpProfileCommandHandler;

    public User ExistingUser { get; }
    public Workout ExistingWorkout { get; }
    public Exercise ExistingExercise { get; }
    public WorkoutHistory ExistingWorkoutHistory { get; }
    public ExerciseHistory ExistingExerciseHistory { get; }

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
        DeleteSetCommandHandler = serviceProvider.GetRequiredService<DeleteSetCommandHandler>();
        AddSetHistoryCommandHandler = serviceProvider.GetRequiredService<AddSetHistoryCommandHandler>();
        MarkSetAsCompletedCommandHandler = serviceProvider.GetRequiredService<MarkSetAsCompletedCommandHandler>();
        MarkSetAsUncompletedCommandHandler = serviceProvider.GetRequiredService<MarkSetAsUncompletedCommandHandler>();
        SetUpProfileCommandHandler = serviceProvider.GetRequiredService<SetUpProfileCommandHandler>();
        
        ExistingUser = CreateExistingUser();
        ExistingWorkout = CreateExistingWorkout();
        ExistingExercise = CreateExistingExercise();
        ExistingWorkoutHistory = CreateExistingWorkoutHistory();
        ExistingExerciseHistory = CreateExistingExerciseHistory();
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

    private WorkoutHistory CreateExistingWorkoutHistory()
    {
        var workoutHistory = WorkoutHistory.Create(30, ExistingWorkout.Id, ExistingUser.Id);

        WorkoutDbContextFixture.WorkoutHistories.Add(workoutHistory);
        WorkoutDbContextFixture.SaveChanges();

        return workoutHistory;
    }

    private ExerciseHistory CreateExistingExerciseHistory()
    {
        var exerciseHistory = ExerciseHistory.Create(ExistingWorkoutHistory.Id, ExistingExercise.Id);

        WorkoutDbContextFixture.ExerciseHistories.Add(exerciseHistory);
        WorkoutDbContextFixture.SaveChanges();

        return exerciseHistory;
    }
}
