using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Configuration;

namespace Domain.DTO;

public class UpdateAuthorDto
{
    [Required]
    [MaxLength(AuthorValidation.NameMaxLength)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(AuthorValidation.CountryMaxLength)]
    public string Country { get; set; } = string.Empty;
}
