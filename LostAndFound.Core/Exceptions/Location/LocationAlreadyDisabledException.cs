using LostAndFound.Core.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Core.Exceptions.Location
{
    public class LocationAlreadyDisabledException : HandledException
    {
        public LocationAlreadyDisabledException() : base(400, "You can't delete an already disabled Location")
        {
        }
    }
}
