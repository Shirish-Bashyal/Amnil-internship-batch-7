namespace AssetManagement.Domain.Entity.BaseEntity;

public abstract class Audit<T>:BaseEntity<T>
{
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }


}
