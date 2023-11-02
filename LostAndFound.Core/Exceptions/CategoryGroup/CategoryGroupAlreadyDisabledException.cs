using LostAndFound.Core.Exceptions.Common;

namespace LostAndFound.Core.Exceptions.CategoryGroup
{
    public class CategoryGroupAlreadyDisabledException : HandledException
    {
        public CategoryGroupAlreadyDisabledException() : base(400, "You can't delete an already disabled Category Groups")
        {
        }
    }
}
