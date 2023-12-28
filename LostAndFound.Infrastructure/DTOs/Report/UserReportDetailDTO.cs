using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.ViolationReport
{
    public class UserReportDetailDTO
    {
        public UserBriefDetailDTO User { get; set; }

        public ReportType Type { get; set; }
    }
}
