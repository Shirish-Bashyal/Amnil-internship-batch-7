using Internship.Api.Models;

namespace Internship.Api.Services
{
    /// <summary>
    /// User service interface demonstrating service layer pattern
    /// </summary>
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User> CreateUserAsync(User user);
        Task<User?> UpdateUserAsync(int id, User user);
        Task<bool> DeleteUserAsync(int id);
        Task<IEnumerable<User>> GetUsersWithOrdersAsync();
        Task<IEnumerable<User>> GetUsersByRoleAsync(string roleName);
        Task<decimal> GetUserTotalSpentAsync(int userId);
    }
}
