using AssetManagementSystem.Domain.Entities.Assets;
using AssetManagementSystem.Domain.Entities.Categories;
using AssetManagementSystem.Domain.Entities.Departments;
using AssetManagementSystem.Domain.Entities.Locations;
using AssetManagementSystem.Domain.Entities.Roles;
using AssetManagementSystem.Domain.Entities.Tags;
using AssetManagementSystem.Domain.Entities.Users;
using AssetManagementSystem.Domain.Interface;
using AssetManagementSystem.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace AssetManagementSystem.Infrastructure.Data;

/// <summary>
///Manages database tables and connections
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    //represents tables in the database
    public DbSet<Asset> Assets { get; set; }
    public DbSet<Tag> Tags { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Department> Departments { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<Location> Locations { get; set; }

    public DbSet<Building> Buildings { get; set; }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e =>
                e.Entity is IDateAuditedEntity
                && (e.State == EntityState.Added || e.State == EntityState.Modified)
            );

        foreach (var entry in entries)
        {
            var entity = (IDateAuditedEntity)entry.Entity;
            if (entry.State == EntityState.Added)
            {
                entity.CreatedDate = DateTime.UtcNow;
            }

            entity.ModifiedDate = DateTime.UtcNow;
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Asset>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name).IsRequired().HasMaxLength(AssetConsts.Name.MaxLength);
            entity.HasIndex(x => x.Name).IsUnique();

            entity
                .Property(x => x.NormalizedName)
                .IsRequired()
                .HasMaxLength(AssetConsts.Name.MaxLength);

            entity
                .Property(x => x.SerialNumber)
                .IsRequired()
                .HasMaxLength(AssetConsts.SerialNumber.MaxLength);
            entity.HasIndex(x => x.SerialNumber).IsUnique();

            entity
                .Property(x => x.NormalizedSerialNumber)
                .IsRequired()
                .HasMaxLength(AssetConsts.SerialNumber.MaxLength);

            entity.Property(x => x.ReceivedDate).IsRequired(false);

            entity.Property(x => x.IsActive).IsRequired();

            entity
                .Property(x => x.Description)
                .IsRequired(false)
                .HasMaxLength(AssetConsts.Description.MaxLength);

            entity
                .HasOne(x => x.Category)
                .WithMany(x => x.Assets)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(x => x.Department)
                .WithMany(x => x.Assets)
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .Property(x => x.Image)
                .IsRequired(false)
                .HasMaxLength(AssetConsts.Image.FileSize);

            entity
                .HasMany(a => a.Tags)
                .WithOne(t => t.Asset)
                .HasForeignKey(t => t.AssetId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            entity
                .HasOne(a => a.Location)
                .WithOne()
                .HasForeignKey<Asset>(a => a.LocationId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            entity
                .HasOne(x => x.User)
                .WithMany(x => x.Assets)
                .HasForeignKey(x => x.UserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity
                .Property(x => x.MacAddress)
                .IsRequired()
                .HasMaxLength(TagConsts.MacAddress.MaxLength);
            entity.HasIndex(x => x.MacAddress).IsUnique();

            entity.Property(x => x.IsActive).IsRequired();
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name).IsRequired().HasMaxLength(DepartmentConsts.Name.MaxLength);
            entity.HasIndex(x => x.Name).IsUnique();

            entity
                .Property(x => x.Description)
                .IsRequired(false)
                .HasMaxLength(DepartmentConsts.Description.MaxLength);

            entity.Property(x => x.IsActive).IsRequired();

            entity.HasData(
                new Department
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "Finance",
                    Description = "Responsible for accounting and budgets",
                    CreatedDate = new DateTime(2025, 10, 14, 13, 30, 5, 816, DateTimeKind.Utc),
                    IsActive = true
                },
                new Department
                {
                    Id = Guid.Parse("11111111-1111-1111-6711-111111111111"),
                    Name = "IT",
                    Description = "Handles computers, networking, and technical infrastructure",
                    CreatedDate = new DateTime(2025, 10, 14, 13, 30, 5, 816, DateTimeKind.Utc),
                    IsActive = true
                },
                new Department
                {
                    Id = Guid.Parse("11987611-1111-1111-1111-111111111111"),
                    Name = "HR",
                    Description = "Manages employee welfare, recruitment, and records",
                    CreatedDate = new DateTime(2025, 10, 14, 13, 30, 5, 816, DateTimeKind.Utc),
                    IsActive = true
                }
            );
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).IsRequired().HasMaxLength(CategoryConsts.Name.MaxLength);

            entity.HasIndex(x => x.Name).IsUnique();

            entity
                .Property(x => x.Description)
                .IsRequired(false)
                .HasMaxLength(CategoryConsts.Description.MaxLength);

            entity.Property(x => x.IsActive).IsRequired();

            entity.HasData(
                new Category
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111402111"),
                    Name = "Vehicles",
                    Description = "Includes company cars, bikes, or trucks",
                    CreatedDate = new DateTime(2025, 10, 14, 13, 30, 5, 816, DateTimeKind.Utc),
                    IsActive = true
                },
                new Category
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "Electronics",
                    Description = "Includes Computers, printers, and projectors",
                    CreatedDate = new DateTime(2025, 10, 14, 13, 30, 5, 816, DateTimeKind.Utc),
                    IsActive = true
                },
                new Category
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Name = "Software",
                    Description = "Includes Licensed software or digital tools",
                    CreatedDate = new DateTime(2025, 10, 14, 13, 30, 5, 816, DateTimeKind.Utc),
                    IsActive = true
                }
            );
        });

        modelBuilder.Entity<Building>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity
                .Property(x => x.Name)
                .IsRequired(true)
                .HasMaxLength(BuildingConsts.Name.MaxLength);
            entity.HasIndex(x => x.Name).IsUnique(true);
            entity.HasData(
                new Building
                {
                    Id = Guid.Parse("33333333-3783-3333-3333-333333333333"),
                    Name = "Block-A"
                },
                new Building
                {
                    Id = Guid.Parse("33333033-3333-3333-3333-233333333333"),
                    Name = "Block-B"
                }
            );
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity
                .Property(x => x.Floor)
                .IsRequired(false)
                .HasMaxLength(LocationConsts.Floor.MaxLength);
            entity
                .Property(x => x.Room)
                .IsRequired(false)
                .HasMaxLength(LocationConsts.Room.MaxLength);
            entity
                .HasOne(x => x.Building)
                .WithMany()
                .HasForeignKey(x => x.BuildingId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).IsRequired(true).HasMaxLength(UserConsts.Name.MaxLength);
            entity.Property(x => x.Email).IsRequired(true).HasMaxLength(UserConsts.Email.MaxLength);
            entity.HasIndex(x => x.Email).IsUnique(true);
            entity
                .HasOne(x => x.Role)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.RoleId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).IsRequired(true).HasMaxLength(RoleConsts.Name.MaxLength);
            entity.HasData(
                new Role
                {
                    Id = Guid.Parse("33330933-3783-3333-3333-333333367333"),
                    Name = "Admin",
                    CreatedDate = new DateTime(2025, 10, 14, 13, 30, 5, 816, DateTimeKind.Utc),
                },
                new Role
                {
                    Id = Guid.Parse("33030933-3783-3233-3333-330933367333"),
                    Name = "Employee",
                    CreatedDate = new DateTime(2025, 10, 14, 13, 30, 5, 816, DateTimeKind.Utc),
                }
            );
        });
    }
}
