using Microsoft.EntityFrameworkCore;
using Internship.Api.Data;
using Internship.Api.Models;

namespace Internship.Api.Services
{
    /// <summary>
    /// User service implementation demonstrating CRUD operations and advanced queries
    /// </summary>
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Basic CRUD Operations
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Where(u => u.IsActive)
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id && u.IsActive);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            user.CreatedAt = DateTime.UtcNow;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> UpdateUserAsync(int id, User user)
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null) return null;

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.IsActive = user.IsActive;

            await _context.SaveChangesAsync();
            return existingUser;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            // Soft delete - just mark as inactive
            user.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        // Advanced Queries with Eager Loading
        public async Task<IEnumerable<User>> GetUsersWithOrdersAsync()
        {
            return await _context.Users
                .Include(u => u.Orders)
                    .ThenInclude(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Where(u => u.IsActive)
                .ToListAsync();
        }

        // Advanced Queries with Filtering and Joins
        public async Task<IEnumerable<User>> GetUsersByRoleAsync(string roleName)
        {
            return await _context.Users
                .Where(u => u.IsActive)
                .Where(u => u.UserRoles.Any(ur => ur.Role.Name == roleName && ur.IsActive))
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .ToListAsync();
        }

        // Aggregation Query
        public async Task<decimal> GetUserTotalSpentAsync(int userId)
        {
            return await _context.Users
                .Where(u => u.Id == userId && u.IsActive)
                .SelectMany(u => u.Orders)
                .Where(o => o.Status != "Cancelled")
                .SumAsync(o => o.TotalAmount);
        }
    }
}
