using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using AttendanceManagementSystem.Contracts.Interfaces;
using AttendanceManagementSystem.Contracts.Interfaces.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AttendanceManagementSystem.Infrastructure.Configurations;

public class BasicAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private protected IBasicAuthValidation _authValidator;

    public BasicAuthHandler(
        IBasicAuthValidation authValidator,
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IUserService userService
    )
        : base(options, logger, encoder, clock)
    {
        _authValidator = authValidator;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return AuthenticateResult.Fail("UnAuthorized");
        }

        string? authorizationHeader = Request.Headers["Authorization"];
        if (string.IsNullOrEmpty(authorizationHeader))
        {
            return AuthenticateResult.Fail("UnAuthorized");
        }
        if (!authorizationHeader.StartsWith("basic ", StringComparison.OrdinalIgnoreCase))
        {
            return AuthenticateResult.Fail("Unauthorized");
        }

        var token = authorizationHeader.Substring(6);
        var credentialAsString = Encoding.UTF8.GetString(Convert.FromBase64String(token));

        var credentials = credentialAsString.Split(":");
        if (credentials?.Length != 2)
        {
            return AuthenticateResult.Fail("Unauthorized");
        }
        var username = credentials[0];
        var password = credentials[1];

        var isValid = _authValidator.IsValid(username, password);
        if (!isValid)
        {
            return AuthenticateResult.Fail("Unauthorized");
        }
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, username) };
        var identity = new ClaimsIdentity(claims, "Basic");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        return AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name));
    }
}
