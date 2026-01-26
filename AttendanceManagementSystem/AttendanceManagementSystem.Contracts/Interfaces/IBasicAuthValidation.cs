using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagementSystem.Contracts.Interfaces;

public interface IBasicAuthValidation
{
    bool IsValid(string userName, string password);
}
