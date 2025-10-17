using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssetManagementSystem.Entity.Entities;
namespace AssetManagementSystem.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
    {
        
    }


    public DbSet<AssetModel> Assets { get; set; }
    public DbSet<TagModel> Tags { get; set; }
    public DbSet<DepartmentModel> Departments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DepartmentModel>().HasData(
        new DepartmentModel
        {
            Id = Guid.NewGuid(),
            Name = "Information Technology",
            Description = "Handles software, hardware, and infrastructure-related assets.",
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System",
            IsActive = true
        },
        new DepartmentModel
        {
            Id = Guid.NewGuid(),
            Name = "Human Resources",
            Description = "Manages employee relations and administrative equipment.",
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System",
            IsActive = true
        },
        new DepartmentModel
        {
            Id = Guid.NewGuid(),
            Name = "Finance",
            Description = "Responsible for accounting systems, finance tools, and related devices.",
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System",
            IsActive = true
        },
        new DepartmentModel
        {
            Id = Guid.NewGuid(),
            Name = "Operations",
            Description = "Oversees logistics, facilities, and maintenance assets.",
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System",
            IsActive = true
        },
        new DepartmentModel
        {
            Id = Guid.NewGuid(),
            Name = "Marketing",
            Description = "Manages branding equipment, digital tools, and promotional materials.",
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System",
            IsActive = true
        }
    );



        // ----------------------
        // BaseModel properties 
        // ----------------------



        void ConfigureBaseModel<TEntity>() where TEntity : BaseModel
        {
            modelBuilder.Entity<TEntity>()
                .Property(e => e.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<TEntity>()
                .Property(e => e.Description)
                .HasMaxLength(1000);

            modelBuilder.Entity<TEntity>()
                .Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<TEntity>()
                .Property(e => e.CreatedBy)
                .HasMaxLength(100);

            modelBuilder.Entity<TEntity>()
                .Property(e => e.ModifiedAt);

            modelBuilder.Entity<TEntity>()
                .Property(e => e.ModifiedBy)
                .HasMaxLength(100);
        }

        ConfigureBaseModel<AssetModel>();
        ConfigureBaseModel<TagModel>();
        ConfigureBaseModel<DepartmentModel>();

        // ----------------------
        // Unique constraints
        // ----------------------

        modelBuilder.Entity<AssetModel>()
            .HasIndex(a => a.SerialNumber)
            .IsUnique();

        modelBuilder.Entity<TagModel>()
            .HasIndex(t => t.MacAddress)
            .IsUnique();

        modelBuilder.Entity<DepartmentModel>()
       .HasIndex(d => d.Name)
       .IsUnique();
        // ----------------------
        // Relationships
        // ----------------------
        // Department → Asset (One-to-Many, no cascade)

        modelBuilder.Entity<AssetModel>()
            .HasOne(a => a.Department)
            .WithMany(d => d.Assets)
            .HasForeignKey(a => a.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);

        // One-to-one: Asset ↔ Tag, FK in Tag
        modelBuilder.Entity<AssetModel>()
            .HasOne(a => a.Tag)
            .WithOne(t => t.Asset)
            .HasForeignKey<TagModel>(t => t.AssetId)  // FK moved to Tag
            .IsRequired(false)                        // optional tag
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AssetModel>()
        .Property(a => a.IsActivated)
        .HasDefaultValue(true);
    }
}

