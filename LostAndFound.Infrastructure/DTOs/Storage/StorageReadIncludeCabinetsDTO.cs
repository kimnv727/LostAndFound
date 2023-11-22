﻿using LostAndFound.Infrastructure.DTOs.Cabinet;
using LostAndFound.Infrastructure.DTOs.Property;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.Storage
{
    public class StorageReadIncludeCabinetsDTO
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public string CampusName { get; set; }
        public bool IsActive { get; set; }
        [JsonProperty("mainStorageManager")]
        public string MainStorageManagerId { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual CampusReadDTO Campus { get; set; }
        public ICollection<CabinetReadDTO> Cabinets { get; set; }
    }
}
