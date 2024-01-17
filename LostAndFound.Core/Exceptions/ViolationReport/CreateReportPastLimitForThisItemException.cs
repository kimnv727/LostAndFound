using LostAndFound.Core.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Core.Exceptions.ViolationReport
{
    public class CreateReportPastLimitForThisItemException : HandledException
    {
        public CreateReportPastLimitForThisItemException() : base(403, "You have already created 3 reports for this Items! ")
        {
        }
    }
}
