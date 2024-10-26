using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostService.Domain.Entities;

namespace PostService.Persistence.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd();

        builder.Property(p => p.UserId)
            .IsRequired();

        builder.Property(p => p.Title)
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(p => p.Description)
            .HasMaxLength(256);

        builder.Property(p => p.ContentUrl)
            .HasMaxLength(2083)
            .IsRequired();

        builder.Property(p => p.ContentType)
            .IsRequired();

        builder.Property(p => p.LikeCount)
            .HasDefaultValue(0);

        builder.Property(p => p.CommentCount)
            .HasDefaultValue(0);

        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();
        
        // Navigation Properties
        builder.HasMany<Like>()
            .WithOne()
            .HasForeignKey(l => l.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany<Comment>()
            .WithOne()
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}