namespace LostAndFound.Infrastructure.DTOs.Item
{
    public class ItemUpdateDTO
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int LocationId { get; set; }
        public int CategoryId { get; set; }
         
    }
}