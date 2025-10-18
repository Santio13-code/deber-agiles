// Persistence/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using Booking.Domain.Entities;

namespace Booking.Infrastructure.Persistence;
public class AppDbContext : DbContext
{
    public DbSet<Service> Services => Set<Service>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Service>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(100);
            e.Property(x => x.DurationMin).IsRequired();
        });

        b.Entity<Client>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(120);
            e.Property(x => x.Email).HasMaxLength(120);
            e.Property(x => x.Phone).HasMaxLength(30);
        });

        b.Entity<Appointment>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Status).IsRequired();
            e.HasIndex(x => new { x.StartUtc, x.EndUtc });
        });
    }
}
