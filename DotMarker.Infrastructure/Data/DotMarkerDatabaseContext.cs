using DotMarker.Domain.Entities;

namespace DotMarker.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

public class DotMarkerDatabaseContext : DbContext
{
    public DotMarkerDatabaseContext(DbContextOptions<DotMarkerDatabaseContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Content> Contents { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.FullName)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<Content>(entity =>
        {
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.CategoryId)
                .IsRequired();
        });

    }
}