namespace WorkoutService.Domain.Entities;

public class Set
{
    public Guid Id { get; private set; }
    public uint Reps { get; private set; }
    public int Weight { get; private set; }
    public bool Completed { get; private set; }
    public Exercise Exercise { get; private set; }
    public Guid ExerciseId { get; private set; }

    private Set(uint reps, int weight, Guid exerciseId)
    {
        Reps = reps;
        Weight = weight;
        Completed = false;
        ExerciseId = exerciseId;
    }

    public static Set Create(uint reps, int weight, Guid exerciseId)
    {
        return new Set(reps, weight, exerciseId);
    }

    public void MarkCompleted()
    {
        Completed = true;
    }
    
#pragma warning disable CS8618 
    // EF Core
    private Set() { }
#pragma warning restore CS8618
}