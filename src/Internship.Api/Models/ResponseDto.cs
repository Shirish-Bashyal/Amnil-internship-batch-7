namespace Internship.Api.Models;

public class ResponseDto<DType>
{
    public bool Success { get; set; }
    public int Status { get; set; }
    public string Message { get; set; }
    public DType Data { get; set; }
}