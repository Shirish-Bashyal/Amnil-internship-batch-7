using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementSystem.Shared.Dtos.Authentication
{
    public class TokenRefreshDto
    {
        public string RefereshToken { get; set; } = string.Empty;

        public bool? isSuccess { get; set; } = true;
        public string Token { get; set; } = string.Empty;
        public string message { get; set; } = string.Empty;
    }

    public class RefereshTokenRequestDto
    {
        public Guid Id { get; set; }
        public string RefereshToken { get; set; } = string.Empty;
    }
}
