namespace AssetManagementSystem.Domain.Interface;

public interface IFullAuditedEntity : IDateAuditedEntity
{
    public Guid CreatedBy { get; set; } //Tracks the id of the user who added the entity

    public Guid? ModifiedBy { get; set; } //Tracks the id of the user who modified the entity

    // public string? DeletedBy { get; set; } //Tracks the id of the user who deleted the entity
    // public DateTime? DeletedDate { get; set; } //Tracks when the entity was Deleted
    // public bool? Deleted { get; set; } //Tracks whether the entity is deleted or not
}
