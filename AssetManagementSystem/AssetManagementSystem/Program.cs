
using AssetManagementSystem.API;
using AssetManagementSystem.Application.Services;
using AssetManagementSystem.Application;
using AssetManagementSystem.Contract.Interfaces;
using AssetManagementSystem.Contract.Interfaces.Service;
using AssetManagementSystem.Infrastructure.Data;
using AssetManagementSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

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
        // Add services to the container.

        //builder.Services.AddDbContext<ApplicationDbContext>(options =>
        //{
        //    options.UseSqlServer(builder.Configuration.GetConnectionString("PrimaryConnnection"));
        //});

        builder.Services.AddControllers();

        MapsterConfig.Configure();

        //service register
        builder.Services.AddScoped<IAssetService,AssetService>();
        builder.Services.AddScoped<IDepartmentService, DepartmentService>();
        builder.Services.AddScoped<ITagService, TagService>();


        //repo register
        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericEfRepository<>));


        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AssetManagementSystem API v1");
                c.RoutePrefix = "swagger"; // Swagger UI at root: https://localhost:7227/
            });
        }


        // Configure the HTTP request pipeline.
        //if (app.Environment.IsDevelopment())
        //{
        //    app.MapOpenApi();
        //}

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
