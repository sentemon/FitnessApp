using WorkoutService.Domain.Enums;

namespace WorkoutService.Domain.Entities;

public class Workout
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public uint DurationInMinutes { get; private set; }
    public DifficultyLevel Level { get; private set; }
    public string Url { get; }
    public string ImageUrl { get; private set; }
    public bool IsCustom { get; private set; }
    public User User { get; private set; }
    public string UserId { get; private set; }
    
    private readonly IList<WorkoutExercise> _workoutExercises = [];
    public IReadOnlyCollection<WorkoutExercise> WorkoutExercises => _workoutExercises.AsReadOnly();

    private Workout(string title, string description, uint durationInMinutes, DifficultyLevel level, string userId)
    {
        Title = title;
        Description = description;
        DurationInMinutes = durationInMinutes;
        Level = level;
        UserId = userId;
        Url = string.Join("-", Title.ToLower().Split(" "));
        IsCustom = true;
    }

    public static Workout Create(string title, string description, uint durationInMinutes, DifficultyLevel level, string userId)
    {
        return new Workout(title, description, durationInMinutes, level, userId);
    }

    public void AddExercise(Exercise exercise)
    {
        if (_workoutExercises.Any(we => we.ExerciseId == exercise.Id))
            throw new InvalidOperationException("This exercise is already added to the workout.");

        var workoutExercise = new WorkoutExercise(Id, exercise.Id);
        _workoutExercises.Add(workoutExercise);
    }

    public void DeleteExercise(Exercise exercise)
    {
        var workoutExercise = _workoutExercises.FirstOrDefault(we => we.ExerciseId == exercise.Id);
        if (workoutExercise is null) 
            return;
        
        _workoutExercises.Remove(workoutExercise);
    }

    public void SetImageUrl(string imageUrl)
    {
        ImageUrl = imageUrl;
    }
    
#pragma warning disable CS8618 
    // EF Core
    private Workout() { }
#pragma warning restore CS8618
}
