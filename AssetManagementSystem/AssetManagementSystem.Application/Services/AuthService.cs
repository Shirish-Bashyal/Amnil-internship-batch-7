using AssetManagementSystem.Contract.Interfaces;
using AssetManagementSystem.Contract.Interfaces.Service;
using AssetManagementSystem.Entity.Entities;
using AssetManagementSystem.Shared.Dtos;
using AssetManagementSystem.Shared.Dtos.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace AssetManagementSystem.Application.Services;



public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IGenericRepository<User> _userrepo;
    private readonly IGenericRepository<Role> _rolerepo;
    public AuthService(IConfiguration configuration, IGenericRepository<User> userrepo, IGenericRepository<Role> rolerepo)
    {
        _userrepo = userrepo;
        _configuration = configuration;
        _rolerepo = rolerepo;
    }


    public async Task<ResponseData<User>> Register(UserDto User)
    {
        try
        {

            if (string.IsNullOrEmpty(User.Password) || string.IsNullOrEmpty(User.UserName) ||
                string.IsNullOrEmpty(User.Phone) || string.IsNullOrEmpty(User.Email) || User.RoleID == null)
            {
                return new ResponseData<User>
                {
                    IsSuccess = false,
                    Message = "give all the required field"

                };
            }


            User user = new User();

            var passwordHashed = new PasswordHasher<User>().HashPassword(user, User.Password);

            user.Phone = User.Phone;
            user.Email = User.Email;
            user.UserName = User.UserName;
            user.PasswordHashed = passwordHashed;
            user.RoleID = User.RoleID;


            bool isPresent = await _userrepo
              .GetQueryable()                  // IQueryable<User>
              .Where(u => u.Email == User.Email)
              .AnyAsync();

            if (isPresent)
            {
                return new ResponseData<User>
                {
                    IsSuccess = false,
                    Message = "User already registered"

                };
            }

            var result=await _userrepo.CreateAsync(user);


            return new ResponseData<User>
            {
                Data = result,
                IsSuccess = true,
                Message = "User Sucessfully added"

            };
        }
        catch (Exception Ex)
        {
            return ResponseData<User>.Exception("some unexpected error occured");
        }


    }

  

    public async Task<TokenRefreshDto> Login(UserLoginDto User)
    {
        try
        {

            if (string.IsNullOrEmpty(User.UserName) || string.IsNullOrEmpty(User.Password))

            {
                return new TokenRefreshDto
                {
                    isSuccess = false,
                    message = "Username or password missing"

                };
            }


            string name = User.UserName;

            User user = await _userrepo.GetByUserNameAsync(name);


            if (user == null)
            {
                return new TokenRefreshDto
                {
                    isSuccess = false,
                    message = "User not found"

                };
            }

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHashed, User.Password)
                == PasswordVerificationResult.Failed) return null;

            if (user.RoleID == null)
            {
                return new TokenRefreshDto
                {
                    isSuccess = false,
                    message = "User has no role assigned."
                };
            }

            // Now safely unwrap the nullable Guid
            Role role = await _rolerepo.GetAsync(user.RoleID.Value);

            if (role == null)
            {
                return new TokenRefreshDto
                {
                    isSuccess = false,
                    message = "Assigned role not found."
                };
            }

            string token = CreateToken(user, role.RoleName);

            return new TokenRefreshDto
            {

                Token = CreateToken(user, role.RoleName),
                RefereshToken = await GenerateAndSaveRefereshToken(user),
            };

        }
        catch (Exception Ex)
        {

            return new TokenRefreshDto
            {
                isSuccess = false,
                message = "unexpected error occured"

            };
        }

    }

    public async Task<TokenRefreshDto> RefereshTokenGenerate(RefereshTokenRequestDto request)
    {


        var data = await _userrepo.GetAsync(request.Id);

        if (data == null || (data.RefereshToken != request.RefereshToken) || data.RefereshTokenExpiry < DateTime.UtcNow)
        {
            return null;
        }


        Role role = await _rolerepo.GetAsync(data.RoleID.Value);


        return new TokenRefreshDto
        {

            Token = CreateToken(data, role.RoleName),
            RefereshToken = await GenerateAndSaveRefereshToken(data),
        };

    }

    private string CreateToken(User user, string role)
    {


        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Role,role)

            };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:Token")!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: _configuration.GetValue<string>("AppSettings:Issuer"),
            audience: _configuration.GetValue<string>("AppSettings:Audience"),
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: creds

            );

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

    }

    private async Task<string> GenerateAndSaveRefereshToken(User user)
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        var refereshToken = Convert.ToBase64String(randomNumber);
        user.RefereshToken = refereshToken;
        user.RefereshTokenExpiry = DateTime.UtcNow.AddDays(1);
        return refereshToken;

    }
}
