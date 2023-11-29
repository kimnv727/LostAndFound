using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.Cabinet;
using LostAndFound.Infrastructure.DTOs.ItemMedia;
using LostAndFound.Infrastructure.DTOs.Location;
using LostAndFound.Infrastructure.DTOs.Media;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.Item
{
    public class ItemReadWithReceiptDTO
    {
        public int Id { get; set; }
        public string FoundUserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [JsonProperty(PropertyName = "locationName")]
        public string LocationLocationName { get; set; }
        public string CategoryName { get; set; }
        public int CabinetId { get; set; }
        public bool IsInStorage { get; set; }
        public ItemStatus ItemStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<ItemMediaLiteReadDTO> ItemMedias { get; set; }
        public int ReceiptId { get; set; }
        public string ReceiverId { get; set; }
        public string? SenderId { get; set; }
        public DateTime ReceiptCreatedDate { get; set; }
        public Guid ReceiptImage { get; set; }
        public ReceiptType ReceiptType { get; set; }
        public LocationReadDTO Location { get; set; }
        public CabinetWithoutItemReadDTO Cabinet { get; set; }
    }
}
