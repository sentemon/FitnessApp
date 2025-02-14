namespace WorkoutService.Domain.Entities;

public class WorkoutHistory
{
    public Guid Id { get; private set; }
    public uint DurationInMinutes { get; private set; }
    public Guid WorkoutId { get; private set; }
    public Workout Workout { get; private set; }
    public string UserId { get; private set; }
    public DateTime PerformedAt { get; private set; }
    
    private readonly List<ExerciseHistory> _exerciseHistories = [];
    public IReadOnlyCollection<ExerciseHistory> ExerciseHistories => _exerciseHistories.AsReadOnly();

    private WorkoutHistory(uint durationInMinutes, Guid workoutId, string userId, DateTime performedAt)
    {
        DurationInMinutes = durationInMinutes;
        WorkoutId = workoutId;
        UserId = userId;
        PerformedAt = performedAt;
    }

    public static WorkoutHistory Create(uint durationInMinutes, Guid workoutId, string userId)
    {
        return new WorkoutHistory(durationInMinutes, workoutId, userId, DateTime.UtcNow);
    }

    public void AddExerciseHistory(ExerciseHistory exerciseHistory)
    {
        _exerciseHistories.Add(exerciseHistory);
    }
    
#pragma warning disable CS8618
    // EF Core
    private WorkoutHistory() { }
#pragma warning restore CS8618
}
