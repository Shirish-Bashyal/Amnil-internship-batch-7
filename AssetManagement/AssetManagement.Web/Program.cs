using AssetManagement.Application.Services;
using AssetManagement.Contract.Repository;
using AssetManagement.Contract.Service;
using AssetManagement.Infrastructure.Data;
using AssetManagement.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddScoped<ICategoryService, CategoryServices>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); // if you're using a generic repo

builder.Services.AddScoped<IAssetService, AssetServices>();





builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Default");

    options
        .UseSqlite(
            connectionString,
            b => b.MigrationsAssembly("AssetManagement.Infrastructure")
        );
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
