using Assisment.Contract;
using Assisment.Contract.Dto;
using Assisment.Contract.DTOs;
using Assisment.Contract.Interface.Repo;
using Assisment.Contract.Interface.Service;
using Assisment.Entity.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assisment.Application.Service;

public class StudentService : IStudentService
{
    private readonly IStudentRepo _repo;

    public StudentService(IStudentRepo repo)
    {
        _repo = repo;
    }

    /// <summary>
    /// Validates the StudentDTO object to ensure all required fields are present.
    /// Returns a ResponseData of string indicating success or the validation message.
    /// </summary>
    private static ResponseData<string> ValidateStudentInput(string? name, string? gender, string? email, string? address)
    {
        if (string.IsNullOrWhiteSpace(name))
            return new ResponseData<string> { Success = false, Message = "Name is required" };

        if (string.IsNullOrWhiteSpace(gender))
            return new ResponseData<string> { Success = false, Message = "Gender is required" };

        if (string.IsNullOrWhiteSpace(email))
            return new ResponseData<string> { Success = false, Message = "Email is required" };

        if (string.IsNullOrWhiteSpace(address))
            return new ResponseData<string> { Success = false, Message = "Address is required" };

        return new ResponseData<string> { Success = true };
    }


    /// <summary>
    /// Creates a new student in the database.
    /// Validates the input DTO and calls the repository to persist the student.
    /// Returns a ResponseData containing the success status and message.
    /// </summary>
    public async Task<ResponseData<StudentDTO>> CreateAsync(CreateStudentDto dto)
    {
        //ResponseData < StudentDTO > response=new ResponseData<StudentDTO> ();

        dto.Name = dto.Name?.Trim();
        dto.Gender = dto.Gender?.Trim();
        dto.Email = dto.Email?.Trim();
        dto.Address = dto.Address?.Trim();

       var validation = ValidateStudentInput(dto.Name, dto.Gender, dto.Email, dto.Address); ;



        if (!validation.Success)
        {
            return new ResponseData<StudentDTO>
            {
                Success = validation.Success,
                Message = validation.Message
            };

        }
        try
        {
            var student = new Student
            {
                Name = dto.Name!,
                Gender = dto.Gender!,
                Email = dto.Email!,
                Address = dto.Address!,
                CreateDate = DateTime.UtcNow,
                IsActive = true
            };

            var result = await _repo.CreateAsync(student);


            return new ResponseData<StudentDTO>
            {
                Success = result.Success,
                Message = result.Message,

            };
        }
        catch (Exception ex)
        {

            throw;
        }

    }

    /// <summary>
    /// Deletes an existing student by ID.
    /// Checks if the ID is provided and calls the repository to delete the student.
    /// Returns a ResponseData indicating the success status and message.
    /// </summary>
    public async Task<ResponseData> DeleteAsync(int id)
    {
        try
        {
            if (id == 0)
            {
                return new ResponseData
                {
                    Success = false,
                    Message = "Student Id not provided."
                };

            }

            var student = await _repo.GetStudentByIdAsync(id);

            if (student == null)
            {
                return new ResponseData
                {
                    Success = false,
                    Message = "Student Id not found."
                };
            }

            var result = await _repo.DeleteAsync(student);
            return result;

        }
        catch (Exception ex)
        {
            throw;
        }

    }

    /// <summary>
    /// Retrieves a paginated list of students from the database.
    /// Accepts a page number and fetches the corresponding page of students.
    /// Returns a ResponseData containing a list of StudentDTOs, success status, and message.
    /// </summary>
    public async Task<ResponseData<List<StudentDTO>>> GetAsync(string? name, string? address)
    {


        ResponseData<List<StudentDTO>> response = new ResponseData<List<StudentDTO>>();

        ResponseData<List<Student>> data = await _repo.GetAsync(name,address);

        if (data.Data == null)
        {
            response.Success = data.Success;
            response.Message = data.Message;
            return response;
        }



        response.Success = data.Success;
        response.Message = data.Message;

        response.Data = data.Data
            .Select(s => new StudentDTO
            {
                Id=s.Id,
                Name = s.Name,
                Gender = s.Gender,
                Email = s.Email,
                Address = s.Address,

            }).ToList();



        return response;

    }

    /// <summary>
    /// Retrieves a single student by ID.
    /// Calls the repository to fetch the student details.
    /// Returns a ResponseData containing the StudentDTO, success status, and message.
    /// </summary>
    public async Task<ResponseData<StudentDTO>> GetStudentByIdAsync(int id)
    {
        var student = await _repo.GetStudentByIdAsync(id);

        if (student == null)
        {
            return new ResponseData<StudentDTO>
            {
                Success = false,
                Message = "Student not found"
            };
        }

        var studentDto = new StudentDTO
        {
            Name = student.Name,
            Gender = student.Gender,
            Email = student.Email,
            Address = student.Address
        };

        return new ResponseData<StudentDTO>
        {
            Success = true,
            Message = "Student retrieved successfully",
            Data = studentDto
        };
    }

    /// <summary>
    /// Updates an existing student by ID.
    /// Validates the input DTO and calls the repository to update the student details.
    /// Returns a ResponseData containing the success status and message.
    /// </summary>
    public async Task<ResponseData<StudentDTO>> UpdateAsync(int id, StudentDTO dto)
    {
        dto.Name = dto.Name?.Trim();
        dto.Gender = dto.Gender?.Trim();
        dto.Email = dto.Email?.Trim();
        dto.Address = dto.Address?.Trim();

        if (id == 0)
        {
            return new ResponseData<StudentDTO>
            {
                Success = false,
                Message = "Id is not given for update"
            };

        }

        var validation = ValidateStudentInput(dto.Name, dto.Gender, dto.Email, dto.Address);

        if (!validation.Success)
        {
            return new ResponseData<StudentDTO>
            {
                Success = validation.Success,
                Message = validation.Message
            };

        }

        var existingStudent = await _repo.GetStudentByIdAsync(id);
        if (existingStudent == null)
        {
            return new ResponseData<StudentDTO>
            {
                Success = false,
                Message = "Student not found"
            };


        }

        // Map updated fields
        existingStudent.Name = dto.Name!;
        existingStudent.Gender = dto.Gender!;
        existingStudent.Email = dto.Email!;
        existingStudent.Address = dto.Address!;
        existingStudent.ModifiedDate = DateTime.UtcNow;

        // Call repo to update
        var isUpdated = await _repo.UpdateAsync(existingStudent);


        return new ResponseData<StudentDTO>
        {
            Success = isUpdated.Success,
            Message = isUpdated.Message
        };

    }


    public async Task<PaginatedResponse<StudentDTO>> GetPaginatedAsync(int pageNumber = 1, int pageSize = 10)
    {
        var query = _repo.GetQueryable();

        var totalRecords = await query.CountAsync();
        var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

        var students = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(s => new StudentDTO
            {
                Id=s.Id,
                Name = s.Name,
                Gender = s.Gender,
                Email = s.Email,
                Address = s.Address
            })
            .ToListAsync();

        return new PaginatedResponse<StudentDTO>
        {
            Success = true,
            Message = $"Page {pageNumber} of {totalPages}",
            Data = students,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            TotalPages = totalPages
        };
    }
}
