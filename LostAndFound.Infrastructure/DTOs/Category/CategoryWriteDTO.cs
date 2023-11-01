﻿using LostAndFound.Core.Enums;

namespace LostAndFound.Infrastructure.DTOs.Category
{
    public class CategoryWriteDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsSensitive { get; set; }
        public ItemValue Value { get; set; }
        public int CategoryGroupId { get; set; }
    }
}
