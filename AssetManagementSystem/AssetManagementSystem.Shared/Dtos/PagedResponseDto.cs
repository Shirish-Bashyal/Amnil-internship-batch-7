namespace AssetManagementSystem.Shared.Dtos;

/// <summary>
/// returns all paged response
/// </summary>
public class PagedResponseDto<T>
{
    public int TotalCount { get; set; }
    public List<T> Items { get; set; } = new List<T>();
}
