using LostAndFound.Core.Exceptions.Common;

namespace LostAndFound.Core.Exceptions.Cabinet
{
    public class CabinetStillHaveItemException : HandledException
    {
        public CabinetStillHaveItemException() : base(400, "Cabinet still have one or more Item(s) active")
        {
        }
    }
}
