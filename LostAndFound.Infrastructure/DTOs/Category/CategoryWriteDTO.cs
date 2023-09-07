using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.Category
{
    public class CategoryWriteDTO
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool? IsActive { get; set; }

    }
}
