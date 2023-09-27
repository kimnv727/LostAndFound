using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LostAndFound.Core.Enums;

namespace LostAndFound.Infrastructure.DTOs.Item
{
    public class ItemWriteDTO
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public string FoundLocation { get; set; }
        
        /*public int CategoryId { get; set; }
        
        public ItemStatus ItemStatus { get; set; }*/
        
    }
}
