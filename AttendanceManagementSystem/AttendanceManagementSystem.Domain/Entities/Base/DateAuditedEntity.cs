using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagementSystem.Domain.Entities.Base;

/// <summary>
///Serve as a base class for entities that auditing information
///Includes the date, the entity was created and the date it was last modified.
/// </summary>
/// <typeparam name="TPrimaryKey"> represents the type of primary key for the entity</typeparam>
public abstract class DateAuditedEntity<TPrimaryKey> : Entity<TPrimaryKey>
{
    public DateTime? AddedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}
