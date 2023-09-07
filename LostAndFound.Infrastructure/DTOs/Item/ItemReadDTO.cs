using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.Item
{
    public class ItemReadDTO
    {
        public int Id { get; set; }

        public string Found_User_Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Found_Location { get; set; }

        public string Category_Id { get; set; }

        public bool? IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTime? DeletedDate { get; set; }

        public Guid? DeletedBy { get; set; }

    }
}
