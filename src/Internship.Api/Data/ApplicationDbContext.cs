using Internship.Api.Constants;
using Internship.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Internship.Api.Data
{
    /// <summary>
    /// ApplicationDbContext - Main database context for Entity Framework Core
    /// Demonstrates DbContext setup, DbSets, and relationship configuration
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets - Represent tables in the database
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Author> Authors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.HasIndex(e => e.Email).IsUnique(); // Unique constraint
                
                // Configure one-to-many relationship with Orders
                entity.HasMany(e => e.Orders)
                      .WithOne(e => e.User)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Product entity
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Price).HasPrecision(10, 2);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            });

            // Configure Order entity
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.OrderNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.TotalAmount).HasPrecision(10, 2);
                entity.HasIndex(e => e.OrderNumber).IsUnique();

                // Configure one-to-many relationship with OrderItems
                entity.HasMany(e => e.OrderItems)
                      .WithOne(e => e.Order)
                      .HasForeignKey(e => e.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure OrderItem entity
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UnitPrice).HasPrecision(10, 2);

                // Configure many-to-one relationship with Product
                entity.HasOne(e => e.Product)
                      .WithMany(e => e.OrderItems)
                      .HasForeignKey(e => e.ProductId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Role entity
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.Name).IsUnique();
            });

            // Configure UserRole junction table
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Configure many-to-one relationship with User
                entity.HasOne(e => e.User)
                      .WithMany(e => e.UserRoles)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Configure many-to-one relationship with Role
                entity.HasOne(e => e.Role)
                      .WithMany(e => e.UserRoles)
                      .HasForeignKey(e => e.RoleId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Composite unique constraint
                entity.HasIndex(e => new { e.UserId, e.RoleId }).IsUnique();
            });


            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(StudentConsts.Name.MaxLength);
                entity.HasIndex(e => e.Name).IsUnique();

                entity.Property(e => e.Email).IsRequired().HasMaxLength(StudentConsts.Email.MaxLength);
                entity.HasIndex(e => e.Email).IsUnique();

                entity.Property(e => e.PhoneNumber).HasMaxLength(StudentConsts.PhoneNumber.MaxLength);
            });



            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin", Description = "Administrator role", IsActive = true },
                new Role { Id = 2, Name = "Customer", Description = "Customer role", IsActive = true },
                new Role { Id = 3, Name = "Manager", Description = "Manager role", IsActive = true }
            );

            // Seed Users
            modelBuilder.Entity<User>().HasData(
                new User 
                { 
                    Id = 1, 
                    FirstName = "John", 
                    LastName = "Doe", 
                    Email = "john.doe@example.com", 
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new User 
                { 
                    Id = 2, 
                    FirstName = "Jane", 
                    LastName = "Smith", 
                    Email = "jane.smith@example.com", 
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            );

            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product 
                { 
                    Id = 1, 
                    Name = "Laptop", 
                    Description = "High-performance laptop", 
                    Price = 999.99m, 
                    StockQuantity = 10,
                    Category = "Electronics",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Product 
                { 
                    Id = 2, 
                    Name = "Mouse", 
                    Description = "Wireless mouse", 
                    Price = 29.99m, 
                    StockQuantity = 50,
                    Category = "Electronics",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Product 
                { 
                    Id = 3, 
                    Name = "Keyboard", 
                    Description = "Mechanical keyboard", 
                    Price = 79.99m, 
                    StockQuantity = 25,
                    Category = "Electronics",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            );

            // Seed UserRoles
            modelBuilder.Entity<UserRole>().HasData(
                new UserRole { Id = 1, UserId = 1, RoleId = 1, IsActive = true, AssignedAt = DateTime.UtcNow },
                new UserRole { Id = 2, UserId = 2, RoleId = 2, IsActive = true, AssignedAt = DateTime.UtcNow }
            );
        }
    }
}
