using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutService.Domain.Entities;

namespace WorkoutService.Persistence.Configurations;

public class SetConfiguration : IEntityTypeConfiguration<Set>
{
    public void Configure(EntityTypeBuilder<Set> builder)
    {
        builder.HasKey(s => s.Id);
            
        builder.Property(s => s.Reps)
            .IsRequired();
            
        builder.Property(s => s.Weight)
            .IsRequired();
            
        builder.Property(s => s.Completed)
            .IsRequired()
            .HasDefaultValue(false);
            
        builder.Property(s => s.ExerciseId)
            .IsRequired();
    }
}