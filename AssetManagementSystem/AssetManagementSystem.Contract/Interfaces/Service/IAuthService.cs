using AssetManagementSystem.Entity.Entities;
using AssetManagementSystem.Shared.Dtos;
using AssetManagementSystem.Shared.Dtos.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementSystem.Contract.Interfaces.Service;

public interface  IAuthService
{
    public Task<ResponseData<User>> Register(UserDto User);

    public Task<TokenRefreshDto> Login(UserLoginDto user);

    
    public Task<TokenRefreshDto> RefereshTokenGenerate(RefereshTokenRequestDto request);
}
