using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementSystem.Shared.Dtos.Authentication
{
    public class UserDto
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }


        public string Phone { get; set; }


        public Guid RoleID { get; set; }
    }
}
