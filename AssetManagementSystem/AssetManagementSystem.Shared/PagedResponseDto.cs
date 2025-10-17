using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementSystem.Shared
{
    
    
        public class PagedResponseDto<T>
        {
            
            public List<T> Items { get; set; } = new List<T>();

            
            public int TotalCount { get; set; }

            public string Message { get; set; }

            public bool IsSuccess { get; set; }
        }
    
}
