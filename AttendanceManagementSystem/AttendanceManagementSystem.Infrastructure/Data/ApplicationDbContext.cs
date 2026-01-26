using AttendanceManagementSystem.Domain.Entities.Application;
using AttendanceManagementSystem.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagementSystem.Infrastructure.Data;

/// <summary>
/// ApplicationDbContext represents Main database context for Entity Framework Core
/// Demonstrates DbContext setup, DbSets, and relationship configuration
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    //represents tables in the database

    public DbSet<User> Users { get; set; }
    public DbSet<AttendanceRecord> AttendanceRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).IsRequired().HasMaxLength(UserConsts.Name.MaxLength);
            entity
                .Property(x => x.PhoneNumber)
                .IsRequired()
                .HasMaxLength(UserConsts.PhoneNumber.MaxLength);
            entity.HasIndex(x => x.PhoneNumber).IsUnique();

            //  one to many relationship with AttendanceRecord

            entity
                .HasMany(x => x.AttendanceRecords)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<AttendanceRecord>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Status).IsRequired();
            entity.Property(x => x.CheckInTime).IsRequired(false);
            entity.Property(x => x.CheckOutTime).IsRequired(false);
            entity.HasIndex(x => new { x.UserId, x.Date }).IsUnique();
        });
    }
}
