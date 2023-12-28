using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.ViolationReport
{
    public class CreateReportDTO
    {
        public ReportWriteDTO ViolationReport { get; set; }

        public string ReportedUserId { get; set; }
    }
}
