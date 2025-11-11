using AssetManagementSystem.Application.Assets;
using AssetManagementSystem.Application.Dashboard;
using AssetManagementSystem.Application.Tags;
using AssetManagementSystem.Contracts.Assets;
using AssetManagementSystem.Contracts.Dashboard;
using AssetManagementSystem.Contracts.Repositories;
using AssetManagementSystem.Contracts.Tags;
using AssetManagementSystem.Infrastructure.Data;
using AssetManagementSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
QuestPDF.Settings.License = LicenseType.Community;

// Configure Entity Framework Core with SQLite with Lazy loading
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Default");

    options
        .UseSqlite(
            connectionString,
            b => b.MigrationsAssembly("AssetManagementSystem.Infrastructure")
        )
        .UseLazyLoadingProxies();
});

//registering in DI container

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericEfRepository<>));

builder.Services.AddScoped<IAssetService, AssetService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
