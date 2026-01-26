namespace Domain.DTO;

public class ServiceResponseDto<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;

    public T? Data { get; set; }
}
