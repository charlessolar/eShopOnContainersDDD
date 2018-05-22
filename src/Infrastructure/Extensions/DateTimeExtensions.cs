using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Extensions
{
    public static class DateTimeExtensions
    {
        public static long ToUnix(this DateTime datetime)
        {
            return new DateTimeOffset(datetime).ToUnixTimeMilliseconds();
        }
        public static DateTime FromUnix(this long milliseconds)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(milliseconds).UtcDateTime;
        }
        // https://stackoverflow.com/a/38064/223547
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek = DayOfWeek.Sunday)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
        /// <summary>
        /// generate random datetime 
        /// </summary>
        public static DateTime RandomDateTimeForward(this DateTime start, TimeSpan min, TimeSpan max)
        {
            var random = new Random();
            var offset = TimeSpan.FromSeconds((random.NextDouble() * max.TotalSeconds) + min.TotalSeconds);
            return start.Add(offset);
        }
        /// <summary>
        /// generate random datetime 
        /// </summary>
        public static DateTime RandomDateTimeBackward(this DateTime start, TimeSpan min, TimeSpan max)
        {
            var random = new Random();
            var offset = TimeSpan.FromSeconds((random.NextDouble() * max.TotalSeconds) + min.TotalSeconds);
            return start.Subtract(offset);
        }
    }
}
