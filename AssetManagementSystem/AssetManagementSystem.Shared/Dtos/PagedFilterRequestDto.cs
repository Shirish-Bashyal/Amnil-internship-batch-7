namespace AssetManagementSystem.Shared.Dtos;

/// <summary>
/// receives pagination,search, filter , sort criteria
/// </summary>
public class PagedFilterRequestDto
{
    public int SkipCount { get; set; }
    public int MaxResultCount { get; set; }

    public string? SearchTerm { get; set; }

    public string? SortOrder { get; set; }
}
