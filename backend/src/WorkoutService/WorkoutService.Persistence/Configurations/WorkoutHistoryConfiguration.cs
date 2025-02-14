using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutService.Domain.Entities;

namespace WorkoutService.Persistence.Configurations;

public class WorkoutHistoryConfiguration : IEntityTypeConfiguration<WorkoutHistory>
{
    public void Configure(EntityTypeBuilder<WorkoutHistory> builder)
    {
        builder.HasKey(wh => wh.Id);

        builder.Property(wh => wh.Id)
            .ValueGeneratedOnAdd();

        builder.Property(wh => wh.UserId)
            .IsRequired();

        builder.Property(wh => wh.PerformedAt)
            .IsRequired();
    }
}