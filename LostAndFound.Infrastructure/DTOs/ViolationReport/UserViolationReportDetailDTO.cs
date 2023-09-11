using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.ViolationReport
{
    public class UserViolationReportDetailDTO
    {
        public UserBriefDetailDTO User { get; set; }

        public ViolationType Type { get; set; }
    }
}
