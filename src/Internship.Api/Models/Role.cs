using System.ComponentModel.DataAnnotations;

namespace Internship.Api.Models
{
    /// <summary>
    /// Role entity representing user roles
    /// Demonstrates many-to-many relationship with User
    /// </summary>
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        // Navigation Properties - Many-to-Many relationship
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
