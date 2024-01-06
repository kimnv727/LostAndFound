using LostAndFound.Core.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Core.Exceptions.ViolationReport
{
    public class CreateReportPastLimitException : HandledException
    {
        public CreateReportPastLimitException() : base(400, "You have already created 3 reports today! Please wait until tomorrow!")
        {
        }
    }
}
