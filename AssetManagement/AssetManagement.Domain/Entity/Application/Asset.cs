using AssetManagement.Domain.Entity.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Domain.Entity.Application;

public class Asset : BaseEntity<Guid>
{
    public string? Name { get; set; }
    public string? SerialNumber { get; set; }   
    public DateTime? CreatedDate { get; set; }
    public decimal Cost { get; set; }   
    public Guid? UserId { get; set; }
    public Guid? TagId { get; set; }
    public Guid? DepartmentId { get; set; }
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
    public Tag? Tag { get; set; }
    public User? User { get; set; }
    public Department? Department { get; set; }
}
