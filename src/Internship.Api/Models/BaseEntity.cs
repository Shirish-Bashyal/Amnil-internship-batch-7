using System.ComponentModel.DataAnnotations;

namespace Internship.Api.Models;

public class BaseEntity<T>
{
    [Key]
    public T Id { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
