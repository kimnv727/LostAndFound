using LostAndFound.Core.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Core.Exceptions.ItemClaim
{
    public class CannotClaimInactiveItem : HandledException
    {
        public CannotClaimInactiveItem() : base(400, "This item is not active and cannot be claimed!")
        {
        }
    }
}
