using LostAndFound.Core.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Core.Exceptions.Location
{
    public class LocationStillHaveItemOrPostException : HandledException
    {
        public LocationStillHaveItemOrPostException() : base(400, "Location still have Post(s) or Item(s) still active")
        {
        }
    }
}
