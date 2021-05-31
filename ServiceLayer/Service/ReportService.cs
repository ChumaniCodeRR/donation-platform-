using InterfaceLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using InterfaceLayer.Models;

namespace ServiceLayer.Service
{
    public class ReportService
    {
        ICanReport report;
        public ReportService(ICanReport report)
        {
            this.report = report;
        }
        public Stream Generate(DateTime? StartDate, DateTime? EndDate, string Key, string clientcode = null)
        {
            return report.GenerateReport(StartDate, EndDate, Key);
        }
      

    }
}