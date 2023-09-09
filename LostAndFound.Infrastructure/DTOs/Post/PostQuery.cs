using LostAndFound.Infrastructure.DTOs.Common;

namespace LostAndFound.Infrastructure.DTOs.Post
{
    public class PostQuery : PaginatedQuery, ISearchTextQuery, IOrderedQuery 
    {
        public string PostUserId { get; set; }

        public string Title { get; set; }

        public string PostContent { get; set; }
        
        //TODO: Query By CreatedDate?

        public string SearchText { get; set; }

        public string OrderBy { get; set; } = "CreatedDate Desc";
    }
}