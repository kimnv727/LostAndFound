using LostAndFound.Core.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Core.Exceptions.Category
{
    public class CategoryAlreadyDisabledException : HandledException
    {
        public CategoryAlreadyDisabledException() : base(400, "You can't delete an already disabled Category")
        {
        }
    }
}
