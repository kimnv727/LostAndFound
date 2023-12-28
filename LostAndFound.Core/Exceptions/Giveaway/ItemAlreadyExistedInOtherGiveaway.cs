using LostAndFound.Core.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Core.Exceptions.Giveaway
{
    public class ItemAlreadyExistedInOtherGiveaway : HandledException
    {
        public ItemAlreadyExistedInOtherGiveaway() : base(400, "This Item Already existed in other non-disabled Giveaway!")
        {
        }
    }
}
