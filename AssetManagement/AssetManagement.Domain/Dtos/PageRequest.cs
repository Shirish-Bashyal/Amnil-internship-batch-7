using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Domain.Dtos;

public class PageRequest
{
    public string? SearchKeyWord { get; set; }
    public string?SortOrder { get; set; }
    public int SkipPageCount { get; set; }
    public int ListCount { get; set; }
}
