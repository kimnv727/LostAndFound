using System;

namespace LostAndFound.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ToVNTime(this DateTime dateTime)
        {
            return dateTime.ToUniversalTime().AddHours(7);
        }

        public static string ToVNTimeString(this DateTime dateTime)
        {
            return dateTime.ToUniversalTime().AddHours(7).ToString("dd/MM/yyyy h:mm tt");
        }

        public static string ToVNDateString(this DateTime dateTime)
        {
            return dateTime.ToUniversalTime().AddHours(7).ToString("dd/MM/yyyy");
        }
    }
}
