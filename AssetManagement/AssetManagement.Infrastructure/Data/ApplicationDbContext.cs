using AssetManagement.Domain.Entity.Application;
using AssetManagement.Shared.Constant;
using Microsoft.EntityFrameworkCore;

namespace AssetManagement.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Department> Departments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Asset>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Description).HasMaxLength(Constraint.Description.MaxLength);
                entity.Property(x => x.Name).HasMaxLength(Constraint.Name.MaxLength);
                entity.HasIndex(x => x.SerialNumber).IsUnique();
                entity.Property(x => x.SerialNumber).IsRequired().HasMaxLength(Constraint.SerialNumber.MaxLength);
                entity.Property(x => x.Cost).IsRequired();
                entity.Property(x => x.TagId).IsRequired();

                entity.HasOne(x => x.Category).WithMany(y => y.Assets).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(z => z.User).WithMany(y => y.Assets).HasForeignKey(z => z.UserId).OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(w => w.Department).WithMany(w => w.Asset).HasForeignKey(w => w.UserId).OnDelete(deleteBehavior: DeleteBehavior.SetNull);
                entity.HasOne(a => a.Tag).WithOne(t => t.Asset).HasForeignKey<Asset>(a => a.TagId).OnDelete(DeleteBehavior.Cascade);

            });
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Name).IsRequired().HasMaxLength(Constraint.Name.MaxLength);
                entity.Property(x => x.Email).IsRequired().HasMaxLength(Constraint.Email.MaxLength);
                entity.HasIndex(x => x.Email).IsUnique();
                entity.HasOne(x => x.Department).WithMany(y => y.User).HasForeignKey(y => y.DepartmentId).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Description).HasMaxLength(Constraint.Description.MaxLength);
                entity.Property(x => x.CategoryName).IsRequired().HasMaxLength(Constraint.Name.MaxLength);
                entity.HasIndex(x => x.CategoryName).IsUnique();
            });
            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.TagId).IsUnique();
                entity.Property(x => x.TagId).IsRequired().HasMaxLength(Constraint.SerialNumber.MaxLength);
            });
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).ValueGeneratedOnAdd();
                entity.Property(x => x.Name).IsRequired().HasMaxLength(Constraint.Name.MaxLength);
                entity.HasIndex(x => x.Name).IsUnique(true);
                entity.HasData(new Department
                {
                    Id = Guid.Parse("3f5c2a6e-8c3b-4b9e-9f4e-2d9a6e7a1f3c"),
                    Name = "IT"
                },
                new Department
                {
                    Id = Guid.Parse("9a1e4c7d-2b6f-4d8a-bc3e-7f1a9d2e4c5b"),
                    Name = "Finance"
                },
                new Department
                {
                    Id = Guid.Parse("6d2f9b3a-1c4e-4e7a-8f9d-3b2c1a7e6d5f"),
                    Name = "Operations"
                },
                new Department
                {
                    Id = Guid.Parse("e7c1a9f2-3d4b-4f6a-9c2e-1a5b7d3f9e8c"),
                    Name = "HR"
                }


                    );
            });
        }
    }

}
