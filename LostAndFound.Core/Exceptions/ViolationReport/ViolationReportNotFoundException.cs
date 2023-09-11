using LostAndFound.Core.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Core.Exceptions.ViolationReport
{
    public class ViolationReportNotFoundException : HandledException
    {
        public ViolationReportNotFoundException() : base(404, "Violation report not found")
        {
        }
    }
}
