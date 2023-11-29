using System;
using System.Collections.Generic;
using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.Cabinet;
using LostAndFound.Infrastructure.DTOs.ItemClaim;
using LostAndFound.Infrastructure.DTOs.ItemMedia;
using LostAndFound.Infrastructure.DTOs.Location;
using LostAndFound.Infrastructure.DTOs.User;
using Newtonsoft.Json;

namespace LostAndFound.Infrastructure.DTOs.Item
{
    public class ItemDetailReadDTO
    {

        public int Id { get; set; }
        
        public string FoundUserId { get; set; }
        
        //public int LocationId { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }

        [JsonProperty(PropertyName = "locationName")]
        public string LocationLocationName { get; set; }

        public string CategoryName { get; set; }

        public bool IsInStorage { get; set; }

        //Status = Pending / Active / Returned / Closed / Rejected
        public ItemStatus ItemStatus { get; set; }

        public DateTime? FoundDate { get; set; }
        
        public DateTime CreatedDate { get; set; }
        public UserReadDTO User { get; set; }

        public ICollection<ItemMediaReadDTO> ItemMedias { get; set; }
        public ICollection<ItemClaimReadDTO> ItemClaims { get; set; }
        public LocationReadDTO Location { get; set; }
        public CabinetWithoutItemReadDTO Cabinet { get; set; }
    }
}
