using AttendanceManagementSystem.Contracts.Interfaces;
using AttendanceManagementSystem.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace AttendanceManagementSystem.Infrastructure.Configurations;

public class ApiKeyHandler : AuthorizationHandler<ApiKeyRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IApiKeyValidation _apiKeyValidation;

    public ApiKeyHandler(
        IHttpContextAccessor httpContextAccessor,
        IApiKeyValidation apiKeyValidation
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _apiKeyValidation = apiKeyValidation;
    }

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ApiKeyRequirement requirement
    )
    {
        string? apiKey = _httpContextAccessor
            ?.HttpContext
            ?.Request
            .Headers[ApiKeyConsts.ApiKeyHeaderName]
            .ToString();
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            context.Fail();
            return Task.CompletedTask;
        }
        if (!_apiKeyValidation.IsValid(apiKey))
        {
            context.Fail();
            return Task.CompletedTask;
        }
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}
