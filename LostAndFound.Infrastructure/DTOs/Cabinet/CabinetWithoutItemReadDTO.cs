using LostAndFound.Infrastructure.DTOs.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.Cabinet
{
    public class CabinetWithoutItemReadDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StorageId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual StorageReadDTO Storage { get; set; }
    }
}
