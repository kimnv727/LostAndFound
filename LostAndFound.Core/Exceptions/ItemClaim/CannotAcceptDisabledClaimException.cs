using LostAndFound.Core.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Core.Exceptions.ItemClaim
{
    public class CannotAcceptDisabledClaimException : HandledException
    {
        public CannotAcceptDisabledClaimException() : base(400, "Cannot accept a disabled claim!")
        {
        }
    }
}
