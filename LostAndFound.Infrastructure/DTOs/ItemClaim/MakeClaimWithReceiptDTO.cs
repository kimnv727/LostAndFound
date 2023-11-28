using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.ItemClaim
{
    public class MakeClaimWithReceiptDTO
    {

        [Required]
        public string ReceiverId { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        public IFormFile ReceiptMedia { get; set; }
    }
}
