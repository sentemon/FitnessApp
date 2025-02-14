namespace WorkoutService.Domain.Entities;

public class SetHistory
{
    public Guid Id { get; private set; }
    public Guid ExerciseHistoryId { get; private set; }
    public ExerciseHistory ExerciseHistory { get; private set; }
    public uint Reps { get; private set; }
    public int Weight { get; private set; }
    public bool Completed { get; private set; }
    public DateTime CompletedAt { get; private set; }

    private SetHistory(Guid exerciseHistoryId, uint reps, int weight)
    {
        ExerciseHistoryId = exerciseHistoryId;
        Reps = reps;
        Weight = weight;
        Completed = false;
    }

    public static SetHistory Create(Guid exerciseHistoryId, uint reps, int weight)
    {
        return new SetHistory(exerciseHistoryId, reps, weight);
    }
    
    public void MarkAsCompleted() => Completed = true;
    public void MarkAsUncompleted() => Completed = false;

#pragma warning disable CS8618
    // EF Core
    private SetHistory() { }
#pragma warning restore CS8618
}