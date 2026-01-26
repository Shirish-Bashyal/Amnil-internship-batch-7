using System.Reflection;
using Application.Enums;
using Contract.Interfaces.Author;
using Contract.Repository;
using Domain.DTO;
using Domain.Entities.Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _authorDapperRepo;
    private readonly IAuthorRepository _authorEfRepo;
    private readonly IAuthorRepository _authorAdoRepo;
    private readonly ILogger<AuthorService> _logger;

    public AuthorService(
        [FromKeyedServices(DataSource.Dapper)] IAuthorRepository authorDapperRepo,
        [FromKeyedServices(DataSource.EfCore)] IAuthorRepository authorEfRepo,
        [FromKeyedServices(DataSource.Ado)] IAuthorRepository authorAdoRepo,
        ILogger<AuthorService> logger
    )
    {
        _authorDapperRepo = authorDapperRepo;
        _authorEfRepo = authorEfRepo;
        _authorAdoRepo = authorAdoRepo;
        _logger = logger;
    }

    public async Task<ServiceResponseDto<GetAuthorDto>> GetByIdAsync(Guid id)
    {
        _logger.LogDebug("Fetching author by id: {AuthorId}", id);

        var author = await _authorAdoRepo.GetByIdAsync(id);
        if (author == null)
        {
            _logger.LogWarning("Author with id {AuthorId} not found", id);
            return new ServiceResponseDto<GetAuthorDto>
            {
                IsSuccess = false,
                Message = "Id not found"
            };
        }

        _logger.LogInformation("Author with id {AuthorId} retrieved successfully", id);
        return new ServiceResponseDto<GetAuthorDto> { Data = author, IsSuccess = true };
    }

    public async Task<ServiceResponseDto<Guid>> CreateAsync(CreateAuthorDto model)
    {
        _logger.LogDebug(
            "Creating new author with Name: {AuthorName}, Country: {Country}",
            model.Name,
            model.Country
        );

        if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.Country))
        {
            _logger.LogWarning("Author creation failed due to missing Name or Country");
            return new ServiceResponseDto<Guid>
            {
                IsSuccess = false,
                Message = "Name and Country cannot be empty"
            };
        }

        try
        {
            var author = new Author
            {
                Name = model.Name.Trim(),
                Country = model.Country.Trim(),
                AddedDate = DateTime.UtcNow
            };

            var authorId = await _authorEfRepo.CreateAsync(author);

            _logger.LogInformation("Author created successfully with Id {AuthorId}", authorId);
            return new ServiceResponseDto<Guid> { IsSuccess = true, Data = authorId };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating author: {AuthorName}", model.Name);
            throw;
        }
    }

    public async Task<ServiceResponseDto<bool>> DeleteAsync(Guid id)
    {
        _logger.LogDebug("Deleting author with Id: {AuthorId}", id);

        try
        {
            var isDeleted = await _authorEfRepo.DeleteAsync(id);
            if (isDeleted)
            {
                _logger.LogInformation("Author with Id {AuthorId} deleted successfully", id);
                return new ServiceResponseDto<bool>
                {
                    IsSuccess = true,
                    Message = "Author deleted",
                    Data = true
                };
            }
            else
            {
                _logger.LogWarning("Author with Id {AuthorId} not found for deletion", id);
                return new ServiceResponseDto<bool>
                {
                    IsSuccess = false,
                    Message = "Author not found",
                    Data = false
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting author with Id {AuthorId}", id);
            throw;
        }
    }

    public async Task<ServiceResponseDto<IEnumerable<GetAuthorDto>>> GetAllAsync()
    {
        _logger.LogDebug("Fetching all authors");

        try
        {
            var authors = await _authorDapperRepo.GetAllAsync();
            _logger.LogInformation(" authors retrieved successfully");
            return new ServiceResponseDto<IEnumerable<GetAuthorDto>>
            {
                Data = authors,
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all authors");
            throw;
        }
    }

    public async Task<ServiceResponseDto<bool>> UpdateAsync(Guid id, UpdateAuthorDto model)
    {
        _logger.LogDebug(
            "Updating author Id {AuthorId} with Name: {Name}, Country: {Country}",
            id,
            model.Name,
            model.Country
        );

        if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.Country))
        {
            _logger.LogWarning("Author update failed due to missing Name or Country");
            return new ServiceResponseDto<bool>
            {
                IsSuccess = false,
                Message = "Name and Country cannot be empty"
            };
        }

        try
        {
            var author = new Author
            {
                Id = id,
                Name = model.Name.Trim(),
                Country = model.Country.Trim()
            };

            var isUpdated = await _authorEfRepo.UpdateAsync(author);
            if (isUpdated)
            {
                _logger.LogInformation("Author with Id {AuthorId} updated successfully", id);
                return new ServiceResponseDto<bool>
                {
                    IsSuccess = true,
                    Message = "Author updated",
                    Data = true
                };
            }
            else
            {
                _logger.LogWarning("Author with Id {AuthorId} not found for update", id);
                return new ServiceResponseDto<bool>
                {
                    IsSuccess = false,
                    Message = "Author not found",
                    Data = false
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating author with Id {AuthorId}", id);
            throw;
        }
    }

    public async Task<ServiceResponseDto<PagedResponse<GetAuthorDto>>> GetByPage(
        int pageNumber,
        int pageSize
    )
    {
        _logger.LogDebug(
            "Fetching authors page {PageNumber} with page size {PageSize}",
            pageNumber,
            pageSize
        );

        try
        {
            var authors = await _authorAdoRepo.GetByPageAsync(pageNumber, pageSize);
            _logger.LogInformation(
                "Page {PageNumber} retrieved successfully with  authors",
                pageNumber
            );

            return new ServiceResponseDto<PagedResponse<GetAuthorDto>>
            {
                IsSuccess = true,
                Data = authors
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching paged authors at page {PageNumber}", pageNumber);
            throw;
        }
    }
}
