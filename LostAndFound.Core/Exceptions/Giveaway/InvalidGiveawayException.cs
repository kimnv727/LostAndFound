using LostAndFound.Core.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Core.Exceptions.Giveaway
{
    public class InvalidGiveawayException : HandledException
    {
        public InvalidGiveawayException() : base(400, "Invalid Giveaway!")
        {
        }
    }
}
