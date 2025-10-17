using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementSystem.Shared.Constrains;

public static class EntityConstrains
{
    public static class BaseEntity
    {
        public const int DescriptionMaxLength = 1000;
        public const int CreatedByMaxLength = 100;
        public const int ModifiedByMaxLength = 100;
    }

    public static class Asset
    {
        public const int AssetNameMaxLength = 200;
        public const int SerialNumberMaxLength = 100;
        public const int AssetCategoryMaxLength = 100;
    }

    public static class Tag
    {
        public const int MacAddressMaxLength = 50;
    }

    public static class Department
    {
        public const int NameMaxLength = 200;
    }
}
