using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Base;

/// <summary>
///Serves as a base class for entites
/// </summary>
/// <typeparam name="TPrimaryKey">represents the type of primary key for the entity</typeparam>
public abstract class Entity<TPrimaryKey>
{
    public TPrimaryKey Id { get; set; }
}
