namespace AssetManagementSystem.Contracts.Dashboard;

/// <summary>
/// Represents details needed for dashboard
/// </summary>
public record DashboardDto
{
    public int Assets { get; set; }

    public int ActiveAssets { get; set; }

    public int Tags { get; set; }
    public int ActiveTags { get; set; }

    public int Categories { get; set; }

    public int Departments { get; set; }
}
