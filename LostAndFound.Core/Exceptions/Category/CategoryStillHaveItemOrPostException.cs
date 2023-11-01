using LostAndFound.Core.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Core.Exceptions.Category
{
    public class CategoryStillHaveItemOrPostException : HandledException
    {
        public CategoryStillHaveItemOrPostException() : base(400, "Category still have Post(s) or Item(s) still active")
        {
        }
    }
}
