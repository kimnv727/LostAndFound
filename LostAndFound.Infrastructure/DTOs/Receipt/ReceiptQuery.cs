using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.Common;
using LostAndFound.Infrastructure.DTOs.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.Receipt
{
    public class ReceiptQuery : PaginatedQuery
    {
        public int Id { get; set; }

        public string ReceiverId { get; set; }

        public string SenderId { get; set; }

        public int ItemId { get; set; }

        public DateTime CreatedDate { get; set; }

        
        public Guid? ReceiptImage { get; set; }

        [DefaultValue(ReceiptType.INSTORGE)]
        public ReceiptType ReceiptType { get; set; }

        
    }
}
