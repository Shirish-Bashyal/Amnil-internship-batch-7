namespace AssetManagementSystem.Shared.Dtos;

/// <summary>
/// Return type of all service
/// </summary>
public class ResponseDto
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;

    public int StatusCode { get; set; }
}

public class ResponseDto<T> : ResponseDto
    where T : class
{
    public T? Data { get; set; }
}
