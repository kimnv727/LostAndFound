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

        public string? Phone{ get; set; }

        //public string? Avatar{ get; set; }

        public string? SchoolId { get; set; }
        public enum CampusSearch
        {
            All,
            HO_CHI_MINH_CAMPUS,
            DA_NANG_CAMPUS,
            HA_NOI_CAMPUS
        }
        [DefaultValue(CampusSearch.All)]
        public CampusSearch Campus { get; set; }

        public enum RoleSearch
        {
            All,
            User,
            All_Manager,
            Manager,
            Storage_Manager
        }
        [DefaultValue(GenderSearch.All)]
        public RoleSearch Role { get; set; }
        public string SearchText { get; set; }

        public string OrderBy { get; set; } = "Email ASC";
    }
}