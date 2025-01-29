using WorkoutService.Domain.Enums;

namespace WorkoutService.Domain.Entities;

public class Exercise
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public DifficultyLevel Level { get; set; }
    public string UserId { get; private set; }
    
    public IReadOnlyCollection<Set> Sets => _sets.AsReadOnly();
    private readonly IList<Set> _sets = [];

    private Exercise(string name, DifficultyLevel level, string userId)
    {
        Name = name;
        Level = level;
        UserId = userId;
    }

    public static Exercise Create(string name, DifficultyLevel level, string userId)
    {
        return new Exercise(name, level, userId);
    }

    public void AddSet(Set set)
    {
        _sets.Add(set);
    }

    public void DeleteSet(Set set)
    {
        _sets.Remove(set);
    }
    
#pragma warning disable CS8618 
    // EF Core
    private Exercise() { }
#pragma warning restore CS8618
}