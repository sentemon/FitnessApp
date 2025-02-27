using System.Text.Json;
using System.Text.Json.Serialization;
using WorkoutService.Domain.Entities;
using WorkoutService.Persistence.Seed.Models;

namespace WorkoutService.Persistence.Seed;

public static class Seed
{
    public static async Task SeedWorkouts(WorkoutDbContext context)
    {
        var workoutsData = await File.ReadAllTextAsync(Directory.GetCurrentDirectory() + "/../WorkoutService.Persistence/Seed/workouts.json");

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter() } 
        };
        
        var workoutDtos = JsonSerializer.Deserialize<List<WorkoutDto>>(workoutsData, options);

        if (workoutDtos == null)
        {
            throw new Exception("Workout data is null.");
        }

        foreach (var workoutDto in workoutDtos)
        {
            var user = User.Create(Guid.NewGuid().ToString(), "Me", "Example", "example", string.Empty);
            context.Users.Add(user);
            
            var workout = Workout.Create(
                workoutDto.Title,
                workoutDto.Description,
                workoutDto.DurationInMinutes,
                workoutDto.Level,
                user.Id
            );
            
            workout.SetImageUrl("example.com/img");

            var exercises = workoutDto.Exercises
                .Select(e => Exercise.Create(e.Name, e.Level, user.Id))
                .ToList();

            context.Workouts.Add(workout);
            context.Exercises.AddRange(exercises);

            var workoutExercises = exercises
                .Select(exercise => new WorkoutExercise(workout, exercise))
                .ToList();

            context.WorkoutExercises.AddRange(workoutExercises);
        }

        await context.SaveChangesAsync();
    }
}