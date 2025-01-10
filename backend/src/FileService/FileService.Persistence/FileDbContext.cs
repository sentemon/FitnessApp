using FileService.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using File = FileService.Domain.Entities.File;

namespace FileService.Persistence;

public class FileDbContext : DbContext
{
    public FileDbContext(DbContextOptions<FileDbContext> options) : base(options)
    {
    }

    public DbSet<File> Files { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new FileConfiguration());
    }
}