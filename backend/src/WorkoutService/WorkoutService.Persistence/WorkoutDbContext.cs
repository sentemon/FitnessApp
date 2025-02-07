using Microsoft.EntityFrameworkCore;
using WorkoutService.Domain.Entities;
using WorkoutService.Persistence.Configurations;

namespace WorkoutService.Persistence;

public class WorkoutDbContext : DbContext
{
    public WorkoutDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Workout> Workouts { get; set; }
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<WorkoutExercise> WorkoutExercises { get; set; }
    public DbSet<Set> Sets { get; set; }

    public DbSet<WorkoutHistory> WorkoutHistories { get; set; }
    public DbSet<ExerciseHistory> ExerciseHistories { get; set; }
    public DbSet<SetHistory> SetHistories { get; set; }
    
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new WorkoutConfiguration());
        modelBuilder.ApplyConfiguration(new ExerciseConfiguration());
        modelBuilder.ApplyConfiguration(new WorkoutExerciseConfiguration());
        modelBuilder.ApplyConfiguration(new SetConfiguration());

        modelBuilder.ApplyConfiguration(new WorkoutHistoryConfiguration());
        modelBuilder.ApplyConfiguration(new ExerciseHistoryConfiguration());
        modelBuilder.ApplyConfiguration(new SetHistoryConfiguration());
        
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}