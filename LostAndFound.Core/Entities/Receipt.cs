using LostAndFound.Core.Entities.Common;
using LostAndFound.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Core.Entities
{
    public class Receipt : ICreatedEntity
    {
        [Key]
        public int Id { get; set; }

        public string? ReceiverId { get; set; }

        public string? SenderId { get; set; }

        [Required]
        [ForeignKey("Item")]
        public int ItemId { get; set; }

        public DateTime CreatedDate { get; set; }

        [ForeignKey("Media")]
        public Guid ReceiptImage { get;set; }

        [Required]
        public ReceiptType ReceiptType { get; set; }

        public virtual Media Media {  get; set; }

        public virtual Item? Item { get; set; }
    }
}
