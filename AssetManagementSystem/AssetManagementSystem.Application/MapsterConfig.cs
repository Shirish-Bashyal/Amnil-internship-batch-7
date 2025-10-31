using AssetManagementSystem.Entity.Entities;
using AssetManagementSystem.Shared.Dtos.Asset;
using AssetManagementSystem.Shared.Dtos.Department;
using Mapster;

namespace AssetManagementSystem.Application;

public static class MapsterConfig
{
    public static void Configure()
    {


        TypeAdapterConfig<AssetModel, BasicAssetDto>.NewConfig()
            .Map(dest => dest.AssetName, src => src.AssetName)
            .Map(dest => dest.SerialNumber, src => src.SerialNumber);



        TypeAdapterConfig<DepartmentModel, DepartmentReadDto>.NewConfig()
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.IsActive, src => src.IsActive)
            .Map(dest => dest.ModifiedBy, src => src.ModifiedBy)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .Map(dest => dest.CreatedBy, src => src.CreatedBy)
            .Map(dest => dest.ModifiedAt, src => src.ModifiedAt)
            .Map(dest => dest.Id, src => src.Id);


        TypeAdapterConfig<AssetModel, AssetReadDto>.NewConfig()
            .Map(dest => dest.AssetName, src => src.AssetName)
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.AssetCategory, src => src.AssetCategory)
            .Map(dest => dest.ReceivedDate, src => src.ReceivedDate)
            .Map(dest => dest.IsActive, src => src.IsActive)
            .Map(dest => dest.IsActivated, src => src.IsActivated)
            .Map(dest => dest.DepartmentId, src => src.DepartmentId)
            .Map(dest => dest.DepartmentName, src => src.Department.Name)
            .Map(dest => dest.TagId, src => src.Tag.Id)
            .Map(dest => dest.TagId, src => src.Tag.MacAddress)
            .Map(dest => dest.SerialNumber, src => src.SerialNumber);


    }

}

