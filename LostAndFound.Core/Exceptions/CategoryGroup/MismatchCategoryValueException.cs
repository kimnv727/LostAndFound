using LostAndFound.Core.Exceptions.Common;

namespace LostAndFound.Core.Exceptions.CategoryGroup
{
    public class MismatchCategoryValueException : HandledException
    {
        public MismatchCategoryValueException() : base(400, "Category group.")
        {
        }
    }
}