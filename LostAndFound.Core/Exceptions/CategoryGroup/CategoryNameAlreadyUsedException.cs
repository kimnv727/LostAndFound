using LostAndFound.Core.Exceptions.Common;

namespace LostAndFound.Core.Exceptions.CategoryGroup
{
    public class CategoryGroupNameAlreadyUsedException : HandledException
    {
        public CategoryGroupNameAlreadyUsedException() : base(400, "Category group with such name existed.")
        {
        }
    }
}