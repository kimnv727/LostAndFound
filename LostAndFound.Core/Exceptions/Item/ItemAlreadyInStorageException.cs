using LostAndFound.Core.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Core.Exceptions.Item
{
    public class ItemAlreadyInStorageException : HandledException
    {
        public ItemAlreadyInStorageException() : base(400, "This item is already in Storage!")
        {
        }
    }
}
