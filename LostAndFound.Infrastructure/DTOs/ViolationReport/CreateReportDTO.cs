using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.ViolationReport
{
    public class CreateReportDTO
    {
        public ViolationReportWriteDTO WriteDTO { get; set; }

        public Guid ReportedUserId { get; set; }
    }
}
