using LostAndFound.Core.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Core.Exceptions.Giveaway
{
    public class CanOnlyUpdateNotStartedGiveawayException : HandledException
    {
        public CanOnlyUpdateNotStartedGiveawayException() : base(400, "Can only update Not Started or Disabled Giveaway.")
        {
        }
    }
}
