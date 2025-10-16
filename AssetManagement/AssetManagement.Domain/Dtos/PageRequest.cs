namespace AssetManagement.Domain.Dtos;

public class PageRequest
{
    public string? SearchKeyWord { get; set; }
    public string? SortOrder { get; set; }
    public int SkipPageCount { get; set; } = 1;
    public int ListCount { get; set; } = 5;
}
