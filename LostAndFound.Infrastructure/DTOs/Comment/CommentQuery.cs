using LostAndFound.Infrastructure.DTOs.Common;

namespace LostAndFound.Infrastructure.DTOs.Comment
{
    public class CommentQuery : PaginatedQuery, IOrderedQuery 
    {
        public string CommentUserId { get; set; }
        public int PostId { get; set; }
        public string CommentContent { get; set; }
        public string CommentPath{ get; set; }
        
        public string OrderBy { get; set; } = "CreatedDate Desc";
    }
}