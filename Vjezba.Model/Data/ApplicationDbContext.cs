using Microsoft.EntityFrameworkCore;
using Vjezba.Model;

namespace Vjezba.Model.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Dog> Dogs { get; set; }
    public DbSet<DogOwner> Owners { get; set; }
    public DbSet<DogWalker> Walkers { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Table per type inheritance for User base class
        modelBuilder.Entity<User>();
        modelBuilder.Entity<DogOwner>().HasBaseType<User>();
        modelBuilder.Entity<DogWalker>().HasBaseType<User>();

        // DogWalker decimal configuration
        modelBuilder.Entity<DogWalker>()
            .Property(w => w.HourlyRate)
            .HasPrecision(18, 2);

        // Dog relationships
        modelBuilder.Entity<Dog>()
            .HasOne(d => d.Owner)
            .WithMany(o => o.Dogs)
            .OnDelete(DeleteBehavior.Restrict);

        // Booking relationships
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Owner)
            .WithMany(o => o.Bookings)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.DogWalker)
            .WithMany(w => w.Bookings)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Booking>()
            .HasMany(b => b.Dogs)
            .WithMany(d => d.Bookings)
            .UsingEntity<Dictionary<string, object>>(
                "BookingDog",
                right => right.HasOne<Dog>()
                    .WithMany()
                    .HasForeignKey("DogsId")
                    .OnDelete(DeleteBehavior.Cascade),
                left => left.HasOne<Booking>()
                    .WithMany()
                    .HasForeignKey("BookingId")
                    .OnDelete(DeleteBehavior.Cascade),
                join =>
                {
                    join.ToTable("BookingDog");
                    join.HasKey("BookingId", "DogsId");
                });

        // Payment relationships and decimal configuration
        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Booking)
            .WithMany(b => b.Payments)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Payment>()
            .Property(p => p.Amount)
            .HasPrecision(18, 2);

        // Review relationships and decimal configuration
        modelBuilder.Entity<Review>()
            .HasOne(r => r.Owner)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.DogWalker)
            .WithMany(w => w.Reviews)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Review>()
            .Property(r => r.Rating)
            .HasPrecision(18, 2);
    }
}
