using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract.Repository;
using Domain.DTO;
using Domain.Entities.Application;
using LibraryManagementApplication.Data;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repository;

public class AuthorAdoRepository : IAuthorRepository
{
    private readonly string _ConnectionString;

    public AuthorAdoRepository(IConfiguration configuration)
    {
        _ConnectionString = configuration.GetConnectionString("Default");
    }

    public async Task<Guid> CreateAsync(Author entity)
    {
        entity.Id = Guid.NewGuid();
        entity.AddedDate = DateTime.UtcNow;
        const string sql =
            @"
        INSERT INTO Authors (Id, Name, Country, AddedDate)
        VALUES (@Id, @Name, @Country,@AddedDate);";

        using var connection = new SqliteConnection(_ConnectionString);

        using SqliteCommand cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("@Id", entity.Id);
        cmd.Parameters.AddWithValue("@Name", entity.Name);
        cmd.Parameters.AddWithValue("@Country", entity.Country);
        cmd.Parameters.AddWithValue("@AddedDate", entity.AddedDate);

        await connection.OpenAsync();

        await cmd.ExecuteNonQueryAsync();
        return entity.Id;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        const string sql = "DELETE FROM Authors WHERE Id = @Id";
        using var connection = new SqliteConnection(_ConnectionString);
        using SqliteCommand cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("@Id", id);
        await connection.OpenAsync();
        int rows = await cmd.ExecuteNonQueryAsync();
        return rows > 0;
    }

    public async Task<IEnumerable<GetAuthorDto>> GetAllAsync()
    {
        const string sql = "SELECT  Name, Country FROM Authors";
        var authors = new List<GetAuthorDto>();
        using var connection = new SqliteConnection(_ConnectionString);
        using var cmd = new SqliteCommand(sql, connection);
        await connection.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            authors.Add(
                new GetAuthorDto
                {
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Country = reader.GetString(reader.GetOrdinal("Country"))
                }
            );
        }
        return authors;
    }

    public async Task<GetAuthorDto?> GetByIdAsync(Guid id)
    {
        const string sql = "SELECT Name, Country FROM Authors WHERE Id=@Id";
        using var connection = new SqliteConnection(_ConnectionString);
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("@Id", id);
        await connection.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new GetAuthorDto
            {
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Country = reader.GetString(reader.GetOrdinal("Country"))
            };
        }
        return null;
    }

    public async Task<bool> UpdateAsync(Author entity)
    {
        entity.ModifiedDate = DateTime.UtcNow;
        const string sql =
            @"
            UPDATE Authors 
            SET Name = @Name, Country = @Country ,ModifiedDate=@ModifiedDate
            WHERE Id = @Id;";
        using var connection = new SqliteConnection(_ConnectionString);
        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("@Id", entity.Id);
        cmd.Parameters.AddWithValue("@Name", entity.Name);
        cmd.Parameters.AddWithValue("@Country", entity.Country);
        cmd.Parameters.AddWithValue("@ModifiedDate", entity.ModifiedDate);
        await connection.OpenAsync();

        var rows = await cmd.ExecuteNonQueryAsync();
        return rows > 0;
    }

    public async Task<PagedResponse<GetAuthorDto>> GetByPageAsync(int pageNumber, int pageSize)
    {
        var response = new PagedResponse<GetAuthorDto>
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };
        const string countSql = "SELECT COUNT(*) FROM Authors";

        const string sql =
            @"
        SELECT Name, Country
        FROM Authors
        ORDER BY Id
        LIMIT @PageSize OFFSET @Offset";
        using var connection = new SqliteConnection(_ConnectionString);

        await connection.OpenAsync();

        using var countCmd = new SqliteCommand(countSql, connection);

        response.TotalCount = Convert.ToInt32(await countCmd.ExecuteScalarAsync());

        using var cmd = new SqliteCommand(sql, connection);
        cmd.Parameters.AddWithValue("@PageSize", pageSize);
        cmd.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);

        var authors = new List<GetAuthorDto>();
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            authors.Add(
                new GetAuthorDto
                {
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Country = reader.GetString(reader.GetOrdinal("Country"))
                }
            );
        }

        response.Items = authors;
        return response;
    }
}
