using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Core.Exceptions.Common
{
    public class NoIdFoundException : HandledException
    {
        public NoIdFoundException() : base(400, "No ID found in request")
        {
        }
    }
}
