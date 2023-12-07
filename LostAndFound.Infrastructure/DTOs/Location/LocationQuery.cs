using LostAndFound.Infrastructure.DTOs.Common;
using System.ComponentModel;

namespace LostAndFound.Infrastructure.DTOs.Location
{
    public class LocationQuery : PaginatedQuery
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public int CampusId { get; set; }
        public enum ActiveStatus
        {
            All,
            Active,
            Disabled
        }
        [DefaultValue(ActiveStatus.All)]
        public ActiveStatus Status { get; set; }
        public string CampusName { get; set; }
        public int Floor { get; set; }
    }
}