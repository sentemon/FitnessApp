using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutService.Domain.Entities;
using WorkoutService.Domain.Enums;

namespace WorkoutService.Persistence.Configurations;

public class WorkoutConfiguration : IEntityTypeConfiguration<Workout>
{
    public void Configure(EntityTypeBuilder<Workout> builder)
    {
        builder.HasKey(w => w.Id);
        
        builder.Property(w => w.Id)
            .ValueGeneratedOnAdd();
            
        builder.Property(w => w.Title)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(w => w.Description)
            .HasMaxLength(500);
            
        builder.Property(w => w.DurationInMinutes)
            .IsRequired();
            
        builder.Property(w => w.Level)
            .HasConversion(
                v => v.ToString(),
                v => (DifficultyLevel)Enum.Parse(typeof(DifficultyLevel), v))
            .IsRequired();
            
        builder.Property(w => w.UserId)
            .IsRequired();
            
        builder.Property(w => w.Url)
            .IsRequired();
        
        builder.Property(w => w.IsCustom)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasMany(w => w.WorkoutExercises)
            .WithOne(we => we.Workout)
            .HasForeignKey(we => we.WorkoutId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}