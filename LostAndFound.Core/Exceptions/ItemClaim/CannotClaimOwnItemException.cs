using LostAndFound.Core.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Core.Exceptions.ItemClaim
{
    public class CannotClaimOwnItemException : HandledException
    {
        public CannotClaimOwnItemException() : base(400, "User cannot claim item they posted!")
        {
        }
    }
}
