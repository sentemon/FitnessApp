using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutService.Domain.Entities;

namespace WorkoutService.Persistence.Configurations;

public class SetHistoryConfiguration : IEntityTypeConfiguration<SetHistory>
{
    public void Configure(EntityTypeBuilder<SetHistory> builder)
    {
        builder.HasKey(sh => sh.Id);
        
        builder.Property(sh => sh.Id)
            .ValueGeneratedOnAdd();

        builder.Property(sh => sh.Reps)
            .IsRequired();

        builder.Property(sh => sh.Weight)
            .IsRequired();

        builder.Property(sh => sh.Completed)
            .IsRequired();
    }
}