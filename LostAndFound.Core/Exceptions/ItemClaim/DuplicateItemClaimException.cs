using LostAndFound.Core.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Core.Exceptions.ItemClaim
{
    public class DuplicateItemClaimException : HandledException
    {
        public DuplicateItemClaimException() : base(400, "User already claimed this item!")
        {
        }
    }
}
