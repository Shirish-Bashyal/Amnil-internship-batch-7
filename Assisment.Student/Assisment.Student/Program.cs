
using Assisment.Application.Service;
using Assisment.Contract.Interface.Repo;
using Assisment.Contract.Interface.Service;
using Assisment.Infrastructure.AuthHandeler;
using Assisment.Infrastructure.Data;
using Assisment.Infrastructure.Repo;
using Assisment.Student.Middleware;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace Assisment.Student
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
            // Add services to the container.



           // builder.Services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("ApiKey", new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
                    Name="X-Api-Key",
                    Type=Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme="ApiKeyScheme",
                    In=Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description="Api key needed to access the endpoints.Example"
                });

                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference=new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type=Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id="ApiKey"
                            }
                        },
                        new string[]{ }
                    }
                    

                });
            });
            
            builder.Services.AddControllers();
           
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IReportService, ReportRenderingService>();
            builder.Services.AddScoped<ApiKeyFilter>();
            builder.Services.AddScoped<IStudentService, StudentService>();

            builder.Services.AddScoped<IStudentRepo, StudentRepo>();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();
            app.UseLogMiddleware();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }
            //app.UseMiddleware<ApiKeyHandler>();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
