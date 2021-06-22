using System;

namespace Core.Helper
{
    public static class DateTimeConvertHelper
    {
        public const long EpochTicks = 621355968000000000;
        public const long TicksPeriod = 10000000;
        public static DateTime LocalToUtcTime(this DateTime date)
        {
            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(date, TimeZoneInfo.Local.Id, TimeZoneInfo.Utc.Id);
        }

        public static long ToSecondsTimestamp(this DateTime date)
        {
            if (date.Year == 9999)
            {
                return 0;
            }

            long ts = (date.Ticks - EpochTicks) / TicksPeriod;
            return ts;
        }
    }
}