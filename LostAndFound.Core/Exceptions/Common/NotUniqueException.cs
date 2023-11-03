using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Core.Exceptions.Common
{
    public class NotUniqueException : HandledException
    {
        public NotUniqueException(string message) : base(400, message)
        {
        }
    }

    public class FieldNotUniqueException : NotUniqueException
    {
        public FieldNotUniqueException(string name) : base(
            $"{name} must be unique")
        {
        }
    }
}
