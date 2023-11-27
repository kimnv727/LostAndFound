using LostAndFound.Core.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Core.Exceptions.Post
{
    public class PostNotActiveException : HandledException
    {
        public PostNotActiveException() : base(400, "You can't comment on non-Active Post!")
        {
        }
    }
}
