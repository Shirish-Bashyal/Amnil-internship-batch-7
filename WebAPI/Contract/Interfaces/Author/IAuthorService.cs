using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DTO;

namespace Contract.Interfaces.Author;

public interface IAuthorService
{
    Task<ServiceResponseDto<Guid>> CreateAsync(CreateAuthorDto author);

    Task<ServiceResponseDto<bool>> UpdateAsync(Guid id, UpdateAuthorDto author);

    Task<ServiceResponseDto<bool>> DeleteAsync(Guid id);

    Task<ServiceResponseDto<IEnumerable<GetAuthorDto>>> GetAllAsync();

    Task<ServiceResponseDto<PagedResponse<GetAuthorDto>>> GetByPage(int pageNumber, int pageSize);

    Task<ServiceResponseDto<GetAuthorDto>> GetByIdAsync(Guid id);
}
