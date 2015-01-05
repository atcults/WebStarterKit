using System;

namespace Common.Base
{
    public class DayDescription
    {
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        
        public DateTime GetDate()
        {
            return new DateTime(Year, Month, Day);
        }

        public bool IsHoliday { get; set; }
        public bool IsExceptionalWorkingDay { get; set; }
        public string Details { get; set; }
    }
}
