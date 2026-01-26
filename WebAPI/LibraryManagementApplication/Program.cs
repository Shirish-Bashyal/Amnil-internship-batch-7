using Application.Enums;
using Application.Services;
using Contract.Interfaces.Author;
using Contract.Repository;
using Infrastructure.Repository;
using LibraryManagementApplication.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Entity Framework Core with SQLite with Lazy loading
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Default");

    options
        .UseSqlite(connectionString, b => b.MigrationsAssembly("Infrastructure"))
        .UseLazyLoadingProxies();
});

// Register the EF Core implementation with the EfCore key
builder.Services.AddKeyedScoped<IAuthorRepository, AuthorEfRepository>(DataSource.EfCore);

// register the Dapper implementation with  Dapper key
builder.Services.AddKeyedScoped<IAuthorRepository, AuthorDapperRepository>(DataSource.Dapper);

//register ADO.Net implementation with key
builder.Services.AddKeyedScoped<IAuthorRepository, AuthorAdoRepository>(DataSource.Ado);

//Register entity Services
builder.Services.AddScoped<IAuthorService, AuthorService>();

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
