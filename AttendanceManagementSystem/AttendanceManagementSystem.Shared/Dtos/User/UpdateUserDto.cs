using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttendanceManagementSystem.Shared.Constants;

namespace AttendanceManagementSystem.Shared.Dtos.User;

public class UpdateUserDto
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(UserConsts.Name.MaxLength)]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name must contain only letters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone Number is required")]
    [StringLength(
        UserConsts.PhoneNumber.MaxLength,
        MinimumLength = UserConsts.PhoneNumber.MaxLength,
        ErrorMessage = "Phone Number must be of length 10."
    )]
    public string PhoneNumber { get; set; } = string.Empty;
}
