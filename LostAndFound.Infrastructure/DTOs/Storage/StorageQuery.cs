using LostAndFound.Infrastructure.DTOs.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.Storage
{
    public class StorageQuery : PaginatedQuery, IOrderedQuery
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public int CampusId { get; set; }
        public enum ActiveStatus
        {
            All,
            Active,
            Disabled
        }
        [DefaultValue(ActiveStatus.All)]
        public ActiveStatus IsActive { get; set; }
        public string MainStorageManagerId { get; set; }
        public string OrderBy { get; set; } = "Location Asc";
    }
}
