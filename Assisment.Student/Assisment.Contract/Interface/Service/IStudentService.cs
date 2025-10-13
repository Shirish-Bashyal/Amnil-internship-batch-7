using Assisment.Contract.Dto;
using Assisment.Contract.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assisment.Contract.Interface.Service;

public interface IStudentService
{
    Task<ResponseData<StudentDTO>> CreateAsync(CreateStudentDto dto);

  
    Task<ResponseData> DeleteAsync(int id);

   
    Task<ResponseData<StudentDTO>> GetStudentByIdAsync(int id);

   
    Task<ResponseData<List<StudentDTO>>> GetAsync(string? name,string? address);

    
    Task<ResponseData<StudentDTO>> UpdateAsync(int id, StudentDTO dto);

    Task<PaginatedResponse<StudentDTO>> GetPaginatedAsync(int pageNumber = 1, int pageSize = 10);
}
