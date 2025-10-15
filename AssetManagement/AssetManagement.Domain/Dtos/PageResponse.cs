using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Domain.Dtos;

public class PageResponse<T>
{
    public int TotalCount {  get; set; }
    public ICollection<T>? Items{ get; set; }
}
