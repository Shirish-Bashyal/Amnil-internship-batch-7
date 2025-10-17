using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementSystem.Shared;

public class PaginationAndFilterRequest
{
    public int SkipCount { get; set; }
    public int MaxResultCount { get; set; }

    public string? SearchTerm { get; set; }

    public string? SortOrder { get; set; }

  
}
