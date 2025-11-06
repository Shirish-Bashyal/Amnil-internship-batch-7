using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementSystem.Shared.Dtos.Authentication
{
    public class UserLoginDto
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
