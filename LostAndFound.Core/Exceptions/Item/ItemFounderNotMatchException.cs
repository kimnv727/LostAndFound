using LostAndFound.Core.Exceptions.Common;

namespace LostAndFound.Core.Exceptions.ItemClaim
{
    public class ItemFounderNotMatchException : HandledException
    {
        public ItemFounderNotMatchException() : base(400, "User is not item founder!")
        {
        }
    }
}
