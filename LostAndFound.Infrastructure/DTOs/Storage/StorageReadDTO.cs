using LostAndFound.Infrastructure.DTOs.Property;
using LostAndFound.Infrastructure.DTOs.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LostAndFound.Infrastructure.DTOs.Storage
{
    public class StorageReadDTO
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public string CampusName { get; set; }
        public bool IsActive { get; set; }
        [JsonProperty("mainStorageManagerId")]
        public string MainStorageManagerId { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual CampusReadDTO Campus { get; set; }
        [JsonProperty("mainStorageManager")]
        public virtual UserReadDTO User { get; set; }
    }
}