using Microsoft.EntityFrameworkCore;

namespace AssetManagementSystem.Infrastructure.Data;

/// <summary>
///Manages database tables and connections
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
}
