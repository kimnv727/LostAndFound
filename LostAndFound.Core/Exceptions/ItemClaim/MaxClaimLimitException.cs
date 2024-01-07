using LostAndFound.Core.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Core.Exceptions.ItemClaim
{
    public class MaxClaimLimitException : HandledException
    {
        public MaxClaimLimitException() : base(400, "You can only have 5 active Claims at a time! Please Unclaim some to proceed!")
        {
        }
    }
}
