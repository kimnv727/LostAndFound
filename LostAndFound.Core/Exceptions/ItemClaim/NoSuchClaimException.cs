using LostAndFound.Core.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Core.Exceptions.ItemClaim
{
    public class NoSuchClaimException : HandledException
    {
        public NoSuchClaimException(): base(400, "User have not claimed this item yet.")
        {
        }
    }
}
