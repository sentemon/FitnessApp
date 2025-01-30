namespace WorkoutService.Domain.Entities;

public class User
{
    public string Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Username { get; private set; }
    public string ImageUrl { get; private set; }

    private readonly List<Workout> _workouts = [];
    public IReadOnlyCollection<Workout> Workouts => _workouts.AsReadOnly();
    
    private readonly List<Exercise> _exercises = [];
    public IReadOnlyCollection<Exercise> Exercises => _exercises.AsReadOnly();

    private User(string id, string firstName, string lastName, string username, string imageUrl)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Username = username;
        ImageUrl = imageUrl;
    }

    public static User Create(string id, string firstName, string lastName, string username, string imageUrl)
    {
        return new User(id, firstName, lastName, username, imageUrl);
    }

    public void AddWorkout(Workout workout)
    {
        if (_workouts.Any(w => w.Id == workout.Id))
            throw new InvalidOperationException("This workout is already added to the user.");
        
        _workouts.Add(workout);
    }

    public void AddExercise(Exercise exercise)
    {
        if (_exercises.Any(e => e.Id == exercise.Id))
            throw new InvalidOperationException("This exercise is already added to the user.");
        
        _exercises.Add(exercise);
    }
    
    public void DeleteWorkout(Workout workout) => _workouts.Remove(workout);
    public void DeleteExercise(Exercise exercise) => _exercises.Remove(exercise);
}