using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.ViolationReport
{
    public class ReportReadDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreatedDate { get; set; }

        public ReportStatus Status { get; set; }

        public ReportCategory Category { get; set; }

        public IEnumerable<UserReportDetailDTO> UserViolationReports { get; set; }
    }
}
