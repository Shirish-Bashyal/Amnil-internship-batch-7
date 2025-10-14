using AssetManagement.Domain.Entity.Application;
using AssetManagement.Shared.Constant;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Infrastructure.Data
{
    public class ApplicationDbContext: DbContext
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
            modelBuilder.Entity<Asset>(entity => {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Name).HasMaxLength(NameConstant.Name.MaxLength);
                entity.HasIndex(x => x.SerialNumber).IsUnique();
                entity.Property(x => x.SerialNumber).IsRequired().HasMaxLength(NameConstant.SerialNumber.MaxLength);
                entity.Property(x => x.Cost).IsRequired();
                entity.HasOne(x => x.Category).WithMany(y => y.Assets).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(z => z.User).WithMany(y => y.Assets).HasForeignKey(z => z.UserId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(w=>w.Department).WithMany(w => w.Asset).HasForeignKey(w => w.UserId).OnDelete(deleteBehavior: DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<User>(entity => {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Name).IsRequired().HasMaxLength(NameConstant.Name.MaxLength);
                entity.Property(x => x.Email).IsRequired().HasMaxLength(NameConstant.Email.MaxLength);
                entity.HasIndex(x => x.Email).IsUnique();
                entity.HasOne(x => x.Department).WithMany(y => y.User).HasForeignKey(y => y.DepartmentId).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.CategoryName).IsRequired().HasMaxLength(NameConstant.Name.MaxLength);
                entity.HasIndex(x => x.CategoryName).IsUnique();
            });
            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.TagId).IsUnique();
                entity.Property(x => x.TagId).IsRequired().HasMaxLength(NameConstant.SerialNumber.MaxLength);
            });
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Name).IsRequired().HasMaxLength(NameConstant.Name.MaxLength);
                entity.HasIndex(x => x.Name).IsUnique(true);

            });
        }
    }
    
}
