using LostAndFound.Core.Exceptions.Common;

namespace LostAndFound.Core.Exceptions.Category
{
    public class CategoryNameAlreadyUsedException : HandledException
    {
        public CategoryNameAlreadyUsedException() : base(400, "Category with such name existed.")
        {
        }
    }
}