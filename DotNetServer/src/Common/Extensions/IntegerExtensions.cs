using System;

namespace Common.Extensions
{
    public static class IntegerExtensions
    {
        public static void Times(this int multiplier, Action<int> action)
        {
            if (multiplier <= 0)
                throw new ArgumentException("Unsupported multiplier. Must be > 0.");

            for (var i = 1; i < multiplier; i++)
            {
                action(i);
            }
        }
    }

    public static class DateTimeExtensions
    {
        public static DateTime FirstDayOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year,dateTime.Month,1);
        }

        public static DateTime LastDayOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year,dateTime.Month,1).AddMonths(1).AddDays(-1);
        }
    }
}