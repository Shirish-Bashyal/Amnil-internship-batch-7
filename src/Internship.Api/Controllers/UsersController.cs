using Microsoft.AspNetCore.Mvc;
using Internship.Api.Services;
using Internship.Api.Models;

namespace Internship.Api.Controllers
{
    /// <summary>
    /// Users API Controller demonstrating CRUD operations and advanced queries
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdUser = await _userService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
        }

        /// <summary>
        /// Update an existing user
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> UpdateUser(int id, User user)
        {
            if (id != user.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedUser = await _userService.UpdateUserAsync(id, user);
            if (updatedUser == null)
                return NotFound();

            return Ok(updatedUser);
        }

        /// <summary>
        /// Delete a user (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Get users with their orders (Eager Loading example)
        /// </summary>
        [HttpGet("with-orders")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersWithOrders()
        {
            var users = await _userService.GetUsersWithOrdersAsync();
            return Ok(users);
        }

        /// <summary>
        /// Get users by role (Advanced Query example)
        /// </summary>
        [HttpGet("by-role/{roleName}")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersByRole(string roleName)
        {
            var users = await _userService.GetUsersByRoleAsync(roleName);
            return Ok(users);
        }

        /// <summary>
        /// Get total amount spent by a user (Aggregation example)
        /// </summary>
        [HttpGet("{id}/total-spent")]
        public async Task<ActionResult<decimal>> GetUserTotalSpent(int id)
        {
            var totalSpent = await _userService.GetUserTotalSpentAsync(id);
            return Ok(new { UserId = id, TotalSpent = totalSpent });
        }
    }
}
