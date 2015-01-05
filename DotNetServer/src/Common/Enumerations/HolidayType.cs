namespace Common.Enumerations
{
    public class HolidayType : Enumeration<HolidayType>
    {
        public HolidayType(string value, string displayName, int displayOrder) : base(value, displayName, displayOrder)
        {
        }

        public static readonly HolidayType Weekly = new HolidayType("W", "This day on each week", 1);
        public static readonly HolidayType Monthly = new HolidayType("M", "This date on each month", 2);
        public static readonly HolidayType Yearly = new HolidayType("Y", "This date on each year", 3);
        public static readonly HolidayType DayOnlly = new HolidayType("D", "On this date only", 4);
        public static readonly HolidayType Exceptional = new HolidayType("E", "We are open on this day", 5);
    }
}