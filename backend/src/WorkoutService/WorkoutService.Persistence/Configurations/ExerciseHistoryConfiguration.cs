using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutService.Domain.Entities;

namespace WorkoutService.Persistence.Configurations;

public class ExerciseHistoryConfiguration : IEntityTypeConfiguration<ExerciseHistory>
{
    public void Configure(EntityTypeBuilder<ExerciseHistory> builder)
    {
        builder.HasKey(eh => eh.Id);
        
        builder.Property(eh => eh.Id)
            .ValueGeneratedOnAdd();

        builder.Property(eh => eh.ExerciseId)
            .IsRequired();
        
        builder.Property(eh => eh.WorkoutHistoryId)
            .IsRequired();
    }
}