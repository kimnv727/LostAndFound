using System.ComponentModel;
using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.Common;

namespace LostAndFound.Infrastructure.DTOs.User
{
    public class UserQuery : PaginatedQuery, ISearchTextQuery, IOrderedQuery 
    {
        public string? FirstName{ get; set; }
        
        public string? LastName{ get; set; }

        public string? FullName { get; set; }
        
        public enum GenderSearch
        {
            All,
            Male,
            Female
        }
        [DefaultValue(GenderSearch.All)]
        public GenderSearch Gender { get; set; }

        public string? Email{ get; set; }

        public string? PhoneNumber{ get; set; }

        public string? Avatar{ get; set; }

        public string SearchText { get; set; }

        public string OrderBy { get; set; } = "Email ASC";
    }
}