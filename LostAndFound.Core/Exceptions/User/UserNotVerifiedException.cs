using LostAndFound.Core.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Core.Exceptions.User
{
    public class UserNotVerifiedException : HandledException
    {
        public UserNotVerifiedException() : base(400, "User is not verified!")
        {
        }
    }
}
