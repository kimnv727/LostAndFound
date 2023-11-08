using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.Receipt
{
    public class ReceiptImageDTO
    {
        public int ItemId { get; set; }
        public IFormFile ReceiptImage { get; set; }
    }
}
