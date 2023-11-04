using LostAndFound.Core.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Core.Exceptions.ItemClaim
{
    public class ItemFounderNotMatchException : HandledException
    {
        public ItemFounderNotMatchException() : base(400, "User is not item founder!")
        {
        }
    }
}
