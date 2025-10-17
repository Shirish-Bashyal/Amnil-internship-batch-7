using AssetManagementSystem.Shared.Constants.Enums;

namespace AssetManagementSystem.Shared.Dtos;

/// <summary>
/// Return type of all service
/// </summary>
public class ResponseDto
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;

    public int StatusCode { get; set; }

    public static ResponseDto BadRequest(string? message = null)
    {
        return new ResponseDto
        {
            IsSuccess = false,
            Message = message ?? "Bad Request",
            StatusCode = 400,
        };
    }

    public static ResponseDto Success(string? message = null)
    {
        return new ResponseDto
        {
            IsSuccess = true,
            Message = message ?? "Operation Successfull",
            StatusCode = 200
        };
    }

    public static ResponseDto InternalServerError(string? message = null)
    {
        return new ResponseDto
        {
            IsSuccess = false,
            Message = message ?? "Internal Server Error",
            StatusCode = 500
        };
    }

    public static ResponseDto NotFound(string? message = null)
    {
        return new ResponseDto
        {
            IsSuccess = false,
            Message = message ?? "Not Found",
            StatusCode = 404
        };
    }
}

public class ResponseDto<T> : ResponseDto
    where T : class
{
    public T? Data { get; set; }

    public static ResponseDto<T> Success(T? data = null, string? message = null)
    {
        return new ResponseDto<T>
        {
            IsSuccess = true,
            Message = message ?? "Operation Successful",
            StatusCode = 200,
            Data = data
        };
    }

    public static ResponseDto<T> Created(T? data = null, string? message = null)
    {
        return new ResponseDto<T>
        {
            IsSuccess = true,
            Message = message ?? "Operation Successful",
            StatusCode = 201,
            Data = data
        };
    }
}
