using LostAndFound.Core.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Core.Exceptions.Item
{
    public class ItemNotActiveOrPendingException : HandledException
    {
        public ItemNotActiveOrPendingException() : base(400, "This item status is not ACTIVE or PENDING!")
        {
        }
    }
}
