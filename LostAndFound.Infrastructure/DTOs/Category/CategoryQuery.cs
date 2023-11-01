using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.Common;
using System.ComponentModel;

namespace LostAndFound.Infrastructure.DTOs.Category
{
    public class CategoryQuery : PaginatedQuery
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public enum ItemValueSearch
        {
            All,
            High,
            Low
        }
        [DefaultValue(ItemValueSearch.All)]
        public ItemValueSearch Value { get; set; }

        public enum SensitiveStatusSearch
        {
            All,
            True,
            False
        }
        [DefaultValue(ItemValueSearch.All)]
        public SensitiveStatusSearch IsSensitive { get; set; }

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