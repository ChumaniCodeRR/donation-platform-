using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceLayer.Models
{
    public interface ICanReport
    {
        Stream GenerateReport(DateTime? StartDate, DateTime? EndDate, string Key);
    }
}
