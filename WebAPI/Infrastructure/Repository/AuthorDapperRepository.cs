using System.Data;
using Contract.Repository;
using Dapper;
using Domain.DTO;
using Domain.Entities.Application;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repository;

public class AuthorDapperRepository : IAuthorRepository
{
    private readonly string _connectionString;

    public AuthorDapperRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default");
    }

    private IDbConnection CreateConnection()
    {
        return new SqliteConnection(_connectionString);
    }

    public async Task<Guid> CreateAsync(Author entity)
    {
        entity.Id = Guid.NewGuid();
        entity.AddedDate = DateTime.UtcNow;

        const string sql =
            @"
        INSERT INTO Authors (Id, Name, Country, AddedDate)
        VALUES (@Id, @Name, @Country,@AddedDate);";
        using var connection = CreateConnection();
        await connection.ExecuteAsync(sql, entity);
        return entity.Id;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        const string sql = "DELETE FROM Authors WHERE Id = @Id";

        using var connection = CreateConnection();
        var rows = await connection.ExecuteAsync(sql, new { Id = id });

        return rows > 0;
    }

    public async Task<IEnumerable<GetAuthorDto>> GetAllAsync()
    {
        const string sql = "SELECT  Name, Country FROM Authors";

        using var connection = CreateConnection();
        return await connection.QueryAsync<GetAuthorDto>(sql);
    }

    public async Task<GetAuthorDto?> GetByIdAsync(Guid id)
    {
        const string sql = "SELECT  Name, Country FROM Authors WHERE Id = @Id";

        using var connection = CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<GetAuthorDto>(sql, new { Id = id });
    }

    public async Task<bool> UpdateAsync(Author entity)
    {
        entity.ModifiedDate = DateTime.UtcNow;
        const string sql =
            @"
            UPDATE Authors 
            SET Name = @Name, Country = @Country ,ModifiedDate=@ModifiedDate
            WHERE Id = @Id;";

        using var connection = CreateConnection();
        var affectedRows = await connection.ExecuteAsync(sql, entity);
        return affectedRows > 0;
    }

    public async Task<PagedResponse<GetAuthorDto>> GetByPageAsync(int pageNumber, int pageSize)
    {
        const string sql =
            @"
        SELECT Name, Country
        FROM Authors
        ORDER BY Id
        LIMIT @PageSize OFFSET @Offset";
        using var connection = CreateConnection();

        var totalCount = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Authors");

        var authors = await connection.QueryAsync<GetAuthorDto>(
            sql,
            new { PageSize = pageSize, Offset = (pageNumber - 1) * pageSize }
        );

        return new PagedResponse<GetAuthorDto>
        {
            Items = authors,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}
