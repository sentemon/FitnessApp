namespace WorkoutService.Domain.Entities;

public class ExerciseHistory
{
    public Guid Id { get; private set; }
    public Guid WorkoutHistoryId { get; private set; }
    public WorkoutHistory WorkoutHistory { get; private set; }
    
    public Guid ExerciseId { get; private set; }
    public Exercise Exercise { get; private set; }

    private readonly List<SetHistory> _setHistories = [];
    public IReadOnlyCollection<SetHistory> SetHistories => _setHistories.AsReadOnly();

    private ExerciseHistory(Guid workoutHistoryId, Guid exerciseId)
    {
        WorkoutHistoryId = workoutHistoryId;
        ExerciseId = exerciseId;
    }

    public static ExerciseHistory Create(Guid workoutHistoryId, Guid exerciseId)
    {
        return new ExerciseHistory(workoutHistoryId, exerciseId);
    }

    public void AddSetHistory(SetHistory setHistory) => _setHistories.Add(setHistory);
    public void DeleteSet(SetHistory setHistory) => _setHistories.Remove(setHistory);

#pragma warning disable CS8618
    // EF Core
    private ExerciseHistory() { }
#pragma warning restore CS8618
}