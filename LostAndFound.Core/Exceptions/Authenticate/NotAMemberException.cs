using LostAndFound.Core.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Core.Exceptions.Authenticate
{
    public class NotAMemberException : HandledException
    {
        public NotAMemberException() : base(403, "This email already belonged to a staff account!")
        {
        }
    }
}
