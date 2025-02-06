using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutService.Domain.Entities;

namespace WorkoutService.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .IsRequired();

        builder.Property(u => u.FirstName)
            .IsRequired();

        builder.Property(u => u.LastName)
            .IsRequired();

        builder.Property(u => u.Username)
            .IsRequired();

        builder.Property(u => u.ImageUrl)
            .IsRequired(false);

        builder.Property(u => u.Weight)
            .IsRequired(false);
        
        builder.Property(u => u.Height)
            .IsRequired(false);
        
        builder.Property(u => u.CurrentGoal)
            .IsRequired(false);
        
        builder.Property(u => u.ActivityLevel)
            .IsRequired(false);
        
        builder.Property(u => u.DateOfBirth)
            .IsRequired(false);
        
        builder.HasMany(u => u.Workouts)
            .WithOne(w => w.User)
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(u => u.Exercises)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}