using LostAndFound.Core.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Core.Exceptions.Item
{
    public class OnlyOneSolvingReportAtATimeException : HandledException
    {
        public OnlyOneSolvingReportAtATimeException() : base(400, "An Item can only has one solving report at a time!")
        {
        }
    }
}
