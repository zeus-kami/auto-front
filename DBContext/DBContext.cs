using auto_front.Models;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
namespace auto_front.DBContext;

public class BusContext : DbContext
{
    public BusContext()
    {
        DotNetEnv.Env.Load();
    }
    public DbSet<Bus> Buses { get; set; }
    public DbSet<Collaborator> Collaborators { get; set; }
    public DbSet<User> Users { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        string host = Environment.GetEnvironmentVariable("DB_HOST");
        string port = Environment.GetEnvironmentVariable("DB_PORT");
        string db = Environment.GetEnvironmentVariable("DB_NAME");
        string user = Environment.GetEnvironmentVariable("DB_USER");
        string pass = Environment.GetEnvironmentVariable("DB_PASSWORD");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        string conn = $"Host={host};Port={port};Database={db};Username={user};Password={pass}";
        optionsBuilder.UseNpgsql(conn);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bus>(e =>
        {
            e.HasKey(b => b.Id);
            e.Property(b => b.Id);
            // ValueGeneratedOnAdd();
            e.Property(b => b.Model)
            .IsRequired()
            .HasMaxLength(100);
            e.Property(b => b.Brand).IsRequired().HasMaxLength(100);
            e.Property(b => b.LicensePlate).IsRequired().HasMaxLength(20);
            e.Property(b => b.Capacity).IsRequired();
            e.Property(b => b.Year).IsRequired();
            e.ToTable("Buses");
        });

        modelBuilder.Entity<Collaborator>(e =>
        {
            e.HasKey(c => c.Id);
            e.Property(c => c.Id);
            e.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
            e.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(100);
            e.ToTable("Collaborators");
        });

        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(u => u.Id);
            e.Property(u => u.Id);
            e.Property(u => u.Login)
                .IsRequired()
                .HasMaxLength(100);
            e.Property(u => u.PasswordHash)
                .IsRequired();
            e.ToTable("Users");
        });



        base.OnModelCreating(modelBuilder);
    }
}
