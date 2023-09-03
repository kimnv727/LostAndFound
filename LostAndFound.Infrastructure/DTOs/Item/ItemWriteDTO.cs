using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.Item
{
    public class ItemWriteDTO
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public string Found_Location { get; set; }

        public bool? IsActive { get; set; }

    }
}
