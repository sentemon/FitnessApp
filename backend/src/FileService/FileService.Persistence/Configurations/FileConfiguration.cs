using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using File = FileService.Domain.Entities.File;

namespace FileService.Persistence.Configurations;

public class FileConfiguration : IEntityTypeConfiguration<File>
{
    public void Configure(EntityTypeBuilder<File> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.Id)
            .ValueGeneratedOnAdd();

        builder.Property(f => f.BlobName)
            .IsRequired();

        builder.Property(f => f.BlobContainerName)
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(f => f.Size)
            .IsRequired();

        builder.Property(f => f.OwnerId)
            .IsRequired();

        builder.Property(f => f.ForeignEntityId)
            .IsRequired();

        builder.Property(f => f.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();
        
        builder.HasIndex(f => new { f.BlobContainerName, f.BlobName })
            .IsUnique();
    }
}