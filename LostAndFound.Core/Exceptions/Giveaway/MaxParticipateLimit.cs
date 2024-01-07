using LostAndFound.Core.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Core.Exceptions.Giveaway
{
    public class MaxParticipateLimit : HandledException
    {
        public MaxParticipateLimit() : base(400, "You can only participate 5 Giveaway at a time!")
        {
        }
    }
}
