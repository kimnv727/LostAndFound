using LostAndFound.Core.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Core.Exceptions.ViolationReport
{
    public class CreateReportException : HandledException
    {
        public CreateReportException() : base(500, "Something went wrong when create report")
        {
        }
    }
}
