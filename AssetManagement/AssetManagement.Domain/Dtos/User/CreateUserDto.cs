using AssetManagement.Shared.Constant;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Domain.Dtos.User;

public class CreateUserDto
{
    [Required(ErrorMessage = "Asset name is required.")]
    [StringLength(Constraints.Name.MaxLength, ErrorMessage = "Name can't exceed 100 characters.")]
    public required string Name { get; set; }
    [Required(ErrorMessage = "Email is required.")]
    [StringLength(Constraints.Email.MaxLength, ErrorMessage = "Name can't exceed 100 characters.")]
    public required string Email { get; set; }
    public Guid DepartmentId { get; set; }
}
