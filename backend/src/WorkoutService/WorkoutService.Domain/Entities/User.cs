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

    private readonly List<WorkoutType> _favoriteWorkoutTypes = [];
    public IReadOnlyCollection<WorkoutType> FavoriteWorkoutTypes => _favoriteWorkoutTypes.AsReadOnly();
    
    private readonly List<Workout> _workouts = [];
    public IReadOnlyCollection<Workout> Workouts => _workouts.AsReadOnly();
    
    private readonly List<Exercise> _exercises = [];
    public IReadOnlyCollection<Exercise> Exercises => _exercises.AsReadOnly();
    
    private readonly List<WorkoutHistory> _workoutHistories = [];
    public IReadOnlyCollection<WorkoutHistory> WorkoutHistories => _workoutHistories.AsReadOnly();

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

    public void SetUpProfile(float weight, float height, Goal currentGoal, ActivityLevel activityLevel, IEnumerable<WorkoutType> favoriteWorkoutTypes, DateTime? dateOfBirth)
    {
        Weight = weight;
        Height = height;
        CurrentGoal = currentGoal;
        ActivityLevel = activityLevel;
        _favoriteWorkoutTypes.Clear();
        _favoriteWorkoutTypes.AddRange(favoriteWorkoutTypes);
        DateOfBirth = dateOfBirth;
    }

    public void AddWorkout(Workout workout) => _workouts.Add(workout);
    public void DeleteWorkout(Workout workout) => _workouts.Remove(workout);
    
    public void AddExercise(Exercise exercise) => _exercises.Add(exercise);
    public void DeleteExercise(Exercise exercise) => _exercises.Remove(exercise);

    public void AddWorkoutHistory(WorkoutHistory workoutHistory) => _workoutHistories.Add(workoutHistory);
    public void DeleteWorkoutHistory(WorkoutHistory workoutHistory) => _workoutHistories.Remove(workoutHistory);
}