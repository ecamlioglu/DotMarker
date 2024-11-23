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
}