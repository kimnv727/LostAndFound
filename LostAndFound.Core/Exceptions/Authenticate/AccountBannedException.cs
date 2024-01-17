using LostAndFound.Core.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Core.Exceptions.Authenticate
{
    public class AccountBannedException : HandledException
    {
        public AccountBannedException() : base(403, "This account is banned!")
        {
        }
    }
}
