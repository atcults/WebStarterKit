using System;
using Common.Enumerations;

namespace Common.Base
{
    public class Holiday
    {
        public string Title { get; set; }
        public HolidayType HolidayType { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }

        public bool IsWorkingException(DateTime date)
        {
            if (!HolidayType.Equals(HolidayType.Exceptional))
            {
                return false;
            }
            return date.Date == new DateTime(Year, Month, Day);
        }

        public bool IsHoliday(DateTime date)
        {
            if (HolidayType.Equals(HolidayType.DayOnlly))
            {
                return date.Date == new DateTime(Year, Month, Day);
            }

            if (HolidayType.Equals(HolidayType.Weekly))
            {
                return date.DayOfWeek == (DayOfWeek)Day;
            }

            if (HolidayType.Equals(HolidayType.Monthly))
            {
                return date.Day == Day;
            }

            if (HolidayType.Equals(HolidayType.Yearly))
            {
                return date.Day == Day && date.Month == Month;
            }

            return false;
        }
    }
}