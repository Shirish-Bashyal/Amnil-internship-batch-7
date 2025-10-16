namespace AssetManagement.Domain.Dtos;

public class PageResponse<T>
{
    public int TotalCount { get; set; }
    public ICollection<T>? Items { get; set; }
}
