namespace WorkoutService.Domain.Entities;

public class WorkoutExercise
{
    public Guid WorkoutId { get; private set; }
    public Workout Workout { get; private set; }

    public Guid ExerciseId { get; private set; }
    public Exercise Exercise { get; private set; }
    
    public WorkoutExercise(Workout workout, Exercise exercise)
    {
        Workout = workout;
        Exercise = exercise;
    }
    
#pragma warning disable CS8618 
    // EF Core
    private WorkoutExercise() { }
#pragma warning restore CS8618
}
