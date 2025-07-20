using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .IsRequired();

        builder.Property(u => u.FirstName)
            .HasMaxLength(128)
            .IsRequired();
        
        builder.Property(u => u.LastName)
            .HasMaxLength(128)
            .IsRequired();

        builder.OwnsOne(u => u.Username, username =>
        {
            username.Property(u => u.Value)
                .HasColumnName("Username")
                .HasMaxLength(30)
                .IsRequired();
        });
        
        builder.OwnsOne(u => u.Email, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("Email")
                .IsRequired()
                .HasMaxLength(320);
        });
        
        builder.Property(u => u.EmailVerified)
            .IsRequired();

        builder.Property(u => u.FollowingCount)
            .HasDefaultValue(0)
            .IsRequired();
        
        builder.Property(u => u.FollowersCount)
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(u => u.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();
    }
}