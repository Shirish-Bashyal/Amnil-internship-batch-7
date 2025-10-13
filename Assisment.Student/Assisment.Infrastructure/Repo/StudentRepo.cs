using Assisment.Contract;
using Assisment.Contract.Interface.Repo;
using Assisment.Entity.Entity;
using Assisment.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Assisment.Infrastructure.Repo;

public class StudentRepo: IStudentRepo
{
    private readonly ApplicationDbContext _context;
    
    public StudentRepo(ApplicationDbContext context)
    {
        _context = context;
       
    }


    public async Task<ResponseData> CreateAsync(Student student)
    {

        
        await _context.AddAsync(student);
        await _context.SaveChangesAsync();
        return new ResponseData()
        {
            Success = true,
            Message = "Data Saved Sucessfully",
        };
        

    }

    public async Task<ResponseData> DeleteAsync(Student data)
    {
        try
        {
            
            _context.Students.Remove(data);
            await _context.SaveChangesAsync();

            return new ResponseData()
            {
                Success = true,
                Message = "Data Sucessfullt Deleted"
            };
        }
        catch (Exception ex) 
        {

            throw;
        }
       
       
    }

    public async Task<ResponseData<List<Student>>> GetAsync(string? name,string? address)
    {


        List<Student> data = await _context.Students.Where(s => (string.IsNullOrEmpty(name) || s.Name.Contains(name)) && (string.IsNullOrEmpty(address) || s.Address.Contains(address))).ToListAsync(); 

        //List<Student> data = await _context.Students.ToListAsync();

        ResponseData<List<Student>> response = new ResponseData<List<Student>>();

        if (data == null || data.Count == 0)
        {

            response.Success = false;
            response.Message = "Data not Fetched ";


            return response;
        }
        response.Data = data;
        response.Success = true;
        response.Message = "Data Fetched Sucessfully";


        return response;
    }

    public async Task<Student> GetStudentByIdAsync(int id)
    {
        return await _context.Students
        .Where(s => s.Id == id && s.IsActive)
        .FirstOrDefaultAsync();

    }


    public async Task<ResponseData> UpdateAsync(Student student)
    {
        try
        {
            _context.Update(student); // or _context.Students.Update(student)
            var updated = await _context.SaveChangesAsync() > 0;

            if (updated)
            {
                return new ResponseData{
                    Success = true,
                    Message = "Student updated successfully"
                };
                
            }
            return new ResponseData
            {
                Success = false,
                Message = "Update failed"
            };
           
            
        }
        catch (Exception ex)
        {
            throw;
        }
    }


    public IQueryable<Student> GetQueryable()
    {
        return _context.Students.Where(s => s.IsActive); 
    }
}
