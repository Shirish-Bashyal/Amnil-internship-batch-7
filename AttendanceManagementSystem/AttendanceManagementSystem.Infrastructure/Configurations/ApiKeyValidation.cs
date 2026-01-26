using AttendanceManagementSystem.Contracts.Interfaces;
using AttendanceManagementSystem.Shared.Constants;
using Microsoft.Extensions.Configuration;

namespace AttendanceManagementSystem.Infrastructure.Configurations;

public class ApiKeyValidation : IApiKeyValidation
{
    private readonly IConfiguration _configuration;

    public ApiKeyValidation(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public bool IsValid(string userKey)
    {
        if (string.IsNullOrWhiteSpace(userKey))
            return false;

        string? apiKey = _configuration[ApiKeyConsts.ApiKeyName];

        if ((apiKey == null) || (apiKey != userKey))
            return false;

        return true;
    }
}
