﻿using System;
using LostAndFound.Core.Enums;
using Newtonsoft.Json;

namespace LostAndFound.Infrastructure.DTOs.Item
{
    public class ItemReadDTO
    {

        public int Id { get; set; }
        
        public string FoundUserId { get; set; }
        
        //public int LocationId { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }

        [JsonProperty(PropertyName = "LocationName")]
        public string LocationLocationName { get; set; }
        public string CategoryName { get; set; }

        //Status = Pending / Active / Returned / Closed / Rejected
        public ItemStatus ItemStatus { get; set; }

        public DateTime? FoundDate { get; set; }
        
        public DateTime CreatedDate { get; set; }
    }
}
