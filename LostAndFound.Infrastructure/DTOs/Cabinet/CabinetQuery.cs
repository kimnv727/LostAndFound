using LostAndFound.Infrastructure.DTOs.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.Cabinet
{
    public class CabinetQuery : PaginatedQuery, IOrderedQuery
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StorageId { get; set; }
        public enum ActiveStatus
        {
            All,
            Active,
            Disabled
        }
        [DefaultValue(ActiveStatus.All)]
        public ActiveStatus IsActive { get; set; }
        public string OrderBy { get; set; } = "Name ASc";
    }
}
