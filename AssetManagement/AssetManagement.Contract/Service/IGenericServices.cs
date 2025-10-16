using AssetManagement.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Contract.Service;

public interface IGenericServices<T>
{
    Task<ServiceResponseDto<T>> GetAsync(string id);
    Task<ServiceResponseDto<IEnumerable<T>>> GetAllAsync();
    Task<ServiceResponseDto<Guid>> CreateAsync(T entity);

    Task<ServiceResponseDto<bool>> DeleteAsync(Guid id);
}
