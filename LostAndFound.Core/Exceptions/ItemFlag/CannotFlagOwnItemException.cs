using LostAndFound.Core.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Core.Exceptions.ItemFlag
{
    public class CannotFlagOwnItemException : HandledException
    {
        public CannotFlagOwnItemException() : base(400, "Cannot flag item you created!")
        {
        }
    }
}
