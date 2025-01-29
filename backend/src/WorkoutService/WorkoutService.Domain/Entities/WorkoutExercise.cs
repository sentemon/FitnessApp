namespace WorkoutService.Domain.Entities;

public class WorkoutExercise
{
    public Guid WorkoutId { get; private set; }
    public Workout Workout { get; private set; }

    public Guid ExerciseId { get; private set; }
    public Exercise Exercise { get; private set; }
    
    public WorkoutExercise(Guid workoutId, Guid exerciseId)
    {
        WorkoutId = workoutId;
        ExerciseId = exerciseId;
    }
    
#pragma warning disable CS8618 
    // EF Core
    private WorkoutExercise() { }
#pragma warning restore CS8618
}
