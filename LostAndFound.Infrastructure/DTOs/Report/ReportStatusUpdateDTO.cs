using LostAndFound.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.Report
{
    public class ReportStatusUpdateDTO
    {
        [DefaultValue(ReportStatus.FAILED)]
        public ReportStatus ReportStatus { get; set; }
        public string? ReportComment { get; set; }
    }
}
