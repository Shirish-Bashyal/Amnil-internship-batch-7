
using AssetManagementSystem.API;
using AssetManagementSystem.Application;
using AssetManagementSystem.Application.Services;
using AssetManagementSystem.Contract.Interfaces;
using AssetManagementSystem.Contract.Interfaces.Service;
using AssetManagementSystem.Infrastructure.Data;
using AssetManagementSystem.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace AssetManagementSystem;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen();


        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)

          .AddJwtBearer(options =>
          {
              options.TokenValidationParameters = new TokenValidationParameters
              {
                  ValidateIssuer = true,
                  ValidateLifetime = true,
                  ValidateAudience = true,
                  ValidateIssuerSigningKey = true,
                  ValidAudience = builder.Configuration["AppSettings:Audience"],
                  ValidIssuer = builder.Configuration["AppSettings:Issuer"],
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!)),
              };

          });

      

        builder.Services.AddControllers();

        MapsterConfig.Configure();

        //service register
        builder.Services.AddScoped<IAssetService,AssetService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IDepartmentService, DepartmentService>();
        builder.Services.AddScoped<ITagService, TagService>();


        //repo register
        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericEfRepository<>));


        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddSwaggerGen((options =>
        {


            // Add JWT Authentication to Swagger
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter your JWT token below"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
                        {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    }
                                },
                                new string[] {}
                            }
                        });
        }));

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            //    (c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AssetManagementSystem API v1");
            //    c.RoutePrefix = "swagger"; // Swagger UI at root: https://localhost:7227/
            //});
        }


        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
