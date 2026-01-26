using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttendanceManagementSystem.Contracts.Interfaces;
using AttendanceManagementSystem.Shared.Constants;
using Microsoft.Extensions.Configuration;

namespace AttendanceManagementSystem.Infrastructure.Configurations;

public class BasicAuthValidation : IBasicAuthValidation
{
    private readonly IConfiguration _configuration;

    public BasicAuthValidation(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public bool IsValid(string userName, string password)
    {
        if ((string.IsNullOrWhiteSpace(password)) || (string.IsNullOrWhiteSpace(userName)))
            return false;

        string? systemUserName = _configuration[
            $"{BasicAuthConsts.BasicAuthName}:{BasicAuthConsts.UserName}"
        ];
        string? systemPassword = _configuration[
            $"{BasicAuthConsts.BasicAuthName}:{BasicAuthConsts.Password}"
        ];

        if (systemPassword == null || systemUserName == null)
            return false;

        if (systemUserName == userName && systemPassword == password)
            return true;

        return false;
    }
}
