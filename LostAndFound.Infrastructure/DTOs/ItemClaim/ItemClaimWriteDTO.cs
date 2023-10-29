﻿using LostAndFound.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.ItemClaim
{
    public class ItemClaimWriteDTO
    {
        public int ItemId { get; set; }
        public string UserId { get; set; }
        public int ClaimStatus { get; set; }
        public DateTime ClaimDate { get; set; }
    }
}
