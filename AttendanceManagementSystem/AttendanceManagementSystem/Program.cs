using AttendanceManagementSystem.API.Configurations;
using AttendanceManagementSystem.Application.Services;
using AttendanceManagementSystem.Contracts.Interfaces;
using AttendanceManagementSystem.Contracts.Interfaces.Attendance;
using AttendanceManagementSystem.Contracts.Interfaces.User;
using AttendanceManagementSystem.Contracts.Repository;
using AttendanceManagementSystem.Infrastructure.Configurations;
using AttendanceManagementSystem.Infrastructure.Data;
using AttendanceManagementSystem.Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IApiKeyValidation, ApiKeyValidation>();
builder.Services.AddTransient<IBasicAuthValidation, BasicAuthValidation>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Entity Framework Core with SQLite with Lazy loading
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Default");

    options
        .UseSqlite(
            connectionString,
            b => b.MigrationsAssembly("AttendanceManagementSystem.Infrastructure")
        )
        .UseLazyLoadingProxies();
});

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericEfRepository<>));

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IAttendanceService, AttendanceService>();
builder
    .Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>("BasicAuthentication", null);

builder
    .Services.AddAuthorizationBuilder()
    .AddPolicy(
        "ApiKeyPolicy",
        policy =>
        {
            policy.AddAuthenticationSchemes("BasicAuthentication");
            policy.Requirements.Add(new ApiKeyRequirement());
        }
    );

builder.Services.AddScoped<IAuthorizationHandler, ApiKeyHandler>();

var app = builder.Build();

//app.UseMiddleware<GlobalExceptionHandler>(); //make seperate exception handler fro production and development env.

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseRouting();

app.UseMiddleware<RequestResponseLoggingMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
