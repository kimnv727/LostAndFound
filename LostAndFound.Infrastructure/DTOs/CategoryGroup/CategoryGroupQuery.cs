using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.Common;
using System.ComponentModel;

namespace LostAndFound.Infrastructure.DTOs.CategoryGroup
{
    public class CategoryGroupQuery : PaginatedQuery
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public enum ActiveStatus
        {
            All,
            Active,
            Disabled
        }
        [DefaultValue(ActiveStatus.All)]
        public ActiveStatus IsActive { get; set; }
    }
}