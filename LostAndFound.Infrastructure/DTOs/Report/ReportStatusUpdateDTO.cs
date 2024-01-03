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
        [DefaultValue(ReportStatus.RESOLVED)]
        public ReportStatus ReportStatus { get; set; }
    }
}
