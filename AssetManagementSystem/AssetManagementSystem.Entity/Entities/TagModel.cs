using AssetManagementSystem.Shared.Constrains;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AssetManagementSystem.Entity.Entities;

public class TagModel:BaseModel
{
    //[Required]
    //[MaxLength(EntityConstrains.Tag.MacAddressMaxLength)]
    //public string MacAddress { get; set; } = string.Empty;

    //// One-to-one relation
    //public AssetModel? Asset { get; set; }

    [Required]
    [MaxLength(EntityConstrains.Tag.MacAddressMaxLength)]
    public string MacAddress { get; set; } = string.Empty;

    // FK pointing to Asset
    public Guid AssetId { get; set; }

    [ForeignKey(nameof(AssetId))]
    public AssetModel Asset { get; set; } = null!;  // required one-to-one
}
