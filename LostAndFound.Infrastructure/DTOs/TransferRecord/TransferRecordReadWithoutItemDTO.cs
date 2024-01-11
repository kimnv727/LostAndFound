using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.Receipt
{
    public class TransferRecordReadWithoutItemDTO
    {
        public int Id { get; set; }

        public string ReceiverId { get; set; }

        public string SenderId { get; set; }

        public int ItemId { get; set; }

        public DateTime CreatedDate { get; set; }

        public Guid ReceiptImage { get; set; }

        public ReceiptType ReceiptType { get; set; }

        public bool IsActive { get; set; }

        public virtual MediaLiteReadDTO Media { get; set; }
    }
}
