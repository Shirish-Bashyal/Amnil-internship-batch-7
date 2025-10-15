namespace AssetManagement.Domain.Dtos;

public class ServiceResponseDto<T>
{
    public bool Success { get; set; }
    public required string Message { get; set; }
    public T? Data { get; set; }
}

public class ServiceResponseDto
{
    public bool Success { get; set; }
    public required string Message { get; set; }
}
