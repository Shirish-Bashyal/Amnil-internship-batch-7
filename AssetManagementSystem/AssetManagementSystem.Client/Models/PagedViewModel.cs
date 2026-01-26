namespace AssetManagementSystem.Client.Models;

/// <summary>
/// represents list of entities with pagination
/// </summary>

public class PagedViewModel<T>
    where T : class
{
    public int TotalItems { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }

    public List<T> Items { get; set; } = new List<T>();
}
