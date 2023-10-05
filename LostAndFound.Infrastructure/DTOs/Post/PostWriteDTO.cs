using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace LostAndFound.Infrastructure.DTOs.Post
{
    public class PostWriteDTO
    {
        //TODO: make allow to add null when write
        [Required]
        [StringLength(500, MinimumLength = 1)]
        public string Title { get; set; }

        [Required]
        [StringLength(Int32.MaxValue, MinimumLength = 1)]
        public string PostContent { get; set; }

        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public int PostLocationId { get; set; }

        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public int PostCategoryId { get; set; }

        [Required]
        public IFormFile[] Medias { get; set; }
    }
}