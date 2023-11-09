using LostAndFound.Core.Exceptions.Common;

namespace LostAndFound.Core.Exceptions.ItemClaim
{
    public class InvalidFlagReason : HandledException
    {
        public InvalidFlagReason() : base(400, "Flag reason not valid")
        {
        }
    }
}
