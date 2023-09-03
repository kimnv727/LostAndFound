using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Core.Exceptions.Common
{
    public class HandledException : Exception
    {
        public int StatusCode { get; set; }

        public HandledException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
