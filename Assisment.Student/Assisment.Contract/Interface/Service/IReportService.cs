using Assisment.Contract.DTOs;
using Assisment.Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assisment.Contract.Interface.Service;

public interface IReportService
{
    Task<byte[]> ReportRenderingAsync(ResponseData<List<StudentDTO>> data);
}
