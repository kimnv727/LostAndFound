using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.Common;
using System.ComponentModel;

namespace LostAndFound.Infrastructure.DTOs.Property
{
    public class CampusQuery : PaginatedQuery
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public bool? IsActive { get; set; }
        /*public enum CampusLocationSearch
        {
            ALL,
            HO_CHI_MINH,
            DA_NANG,
            HA_NOI,
            CAN_THO
        }
        [DefaultValue(CampusLocationSearch.ALL)]
        public CampusLocationSearch CampusLocation { get; set; }*/
    }
}