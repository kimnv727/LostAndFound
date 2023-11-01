using LostAndFound.Core.Exceptions.Common;

namespace LostAndFound.Core.Exceptions.CategoryGroup
{
    public class CategoryGroupStillHaveActiveCategory : HandledException
    {
        public CategoryGroupStillHaveActiveCategory() : base(400, "Category Group still have active Category")
        {
        }
    }
}
