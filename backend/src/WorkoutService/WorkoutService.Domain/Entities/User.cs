using WorkoutService.Domain.Enums;

namespace WorkoutService.Domain.Entities;

public class User
{
    public string Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Username { get; private set; }
    public string ImageUrl { get; private set; }
    public float? Weight { get; private set; }
    public float? Height { get; private set; }
    public Goal? CurrentGoal { get; private set; }
    public ActivityLevel? ActivityLevel { get; private set; }
    public DateTime? DateOfBirth { get; private set; }

    private readonly List<WorkoutType>? _favoriteWorkoutTypes = null;
    public IReadOnlyCollection<WorkoutType>? FavoriteWorkoutTypes => _favoriteWorkoutTypes?.AsReadOnly();
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

    public void SetUpProfile(float weight, float height, Goal currentGoal, ActivityLevel activityLevel, IEnumerable<WorkoutType> favoriteWorkoutTypes, DateTime? dateOfBirth = null)
    {
        if (weight <= 0)
            throw new ArgumentException("Weight must be a positive number.", nameof(weight));
        if (height <= 0)
            throw new ArgumentException("Height must be a positive number.", nameof(height));
        
        Weight = weight;
        Height = height;
        CurrentGoal = currentGoal;
        ActivityLevel = activityLevel;
        _favoriteWorkoutTypes?.Clear();
        _favoriteWorkoutTypes?.AddRange(favoriteWorkoutTypes);
        DateOfBirth = dateOfBirth;
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