using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Internship.Api.Models
{
    /// <summary>
    /// User entity representing a user in the system
    /// Demonstrates basic entity properties and relationships
    /// </summary>
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        // Navigation Properties - One-to-Many relationship
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        // Computed property (not stored in database)
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
    }
}
