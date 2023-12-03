using System;
using System.ComponentModel.DataAnnotations;

namespace LostAndFound.Infrastructure.DTOs.Post
{
    public class PostUpdateDTO
    {
        [Required]
        [StringLength(500, MinimumLength = 1)]
        public string Title { get; set; }
        [Required]
        [StringLength(Int32.MaxValue, MinimumLength = 1)]
        public string PostContent { get; set; }
        public string? PostLocation { get; set; }
        public string? PostCategory { get; set; }
        public string? LostDateFrom { get; set; }
        public string? LostDateTo { get; set; }
    }
}