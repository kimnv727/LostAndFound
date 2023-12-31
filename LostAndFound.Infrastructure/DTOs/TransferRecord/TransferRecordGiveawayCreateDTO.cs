using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.Media;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.Receipt
{
    public class TransferRecordGiveawayCreateDTO
    {
        [Required]
        public string ReceiverId { get; set; }

        [Required]
        public int GiveawayId { get; set; }

    }
}
