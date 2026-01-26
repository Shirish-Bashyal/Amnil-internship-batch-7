using LibraryManagement.Model;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Data;

/// <summary>
/// ApplicationDbContext represents Main database context for Entity Framework Core
/// Demonstrates DbContext setup, DbSets, and relationship configuration
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    //represents tables in the database
    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Borrower> Borrowers { get; set; }

    public DbSet<BorrowerBook> BorrowerBooks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Country).IsRequired().HasMaxLength(50);

            //  one to many relationship with Books
            entity
                .HasMany(e => e.Books)
                .WithOne(e => e.Author)
                .HasForeignKey(e => e.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Title).IsRequired().HasMaxLength(100);

            entity.HasIndex(e => e.ISBN).IsUnique();
        });

        modelBuilder.Entity<Borrower>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<BorrowerBook>(entity =>
        {
            entity.HasKey(e => e.Id);

            // Many to one relationship with Borrower
            entity
                .HasOne(e => e.Borrower)
                .WithMany(b => b.BorrowerBooks)
                .HasForeignKey(e => e.BorrowerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Many to one relationship with Book
            entity
                .HasOne(e => e.Book)
                .WithMany(bk => bk.BorrowerBooks)
                .HasForeignKey(e => e.BookId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
