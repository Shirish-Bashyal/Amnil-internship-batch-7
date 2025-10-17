

namespace AssetManagementSystem.Shared.Dtos;

public class ResponseData<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;

    public T? Data { get; set; }


    public ResponseData() { }

  
   
    public ResponseData(string message)
    {
        IsSuccess = false;
        Message = message;
    }

    public static ResponseData<T> BadRequest(string message=null)
    {
        return new ResponseData<T>
        {
            IsSuccess = false,
            Message = message 
        };
    }

    public static ResponseData<T> Exception(string message = null)
    {
        return new ResponseData<T>
        {
            IsSuccess = false,
            Message = message
        };
    }
    public static ResponseData<T> Success(string message = null)
    {
        return new ResponseData<T>
        {
            IsSuccess = true,
            Message = message
        };
    }
}

