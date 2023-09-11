using System.ComponentModel;
using LostAndFound.Core.Enums;

namespace LostAndFound.Infrastructure.DTOs.Post
{
    public class PostStatusUpdateDTO
    {
        [DefaultValue(PostStatus.ACTIVE)]
        public PostStatus PostStatus { get; set; }
    }
}