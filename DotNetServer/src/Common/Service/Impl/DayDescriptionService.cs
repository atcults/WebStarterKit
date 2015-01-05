using System;
using System.Collections.Generic;
using System.Linq;
using Common.Base;
using Common.Enumerations;

namespace Common.Service.Impl
{
    public class DayDescriptionService : IDayDescriptionService
    {
        private static readonly object Lock = new object();
        private static readonly List<DayDescription> DaysTable = new List<DayDescription>(500);
        private static Holiday[] _holidays = new Holiday[0];

        public void SetHolidays(Holiday[] holidays)
        {
            lock (Lock)
            {
                _holidays = holidays;
            }
        }

        public DayDescription[] GetDays(DateTime startDate, DateTime endDate)
        {
            lock (Lock)
            {
                if (DaysTable.Count == 0)
                {
                    DaysTable.AddRange(GetDayDescriptions(startDate, endDate));
                }
                else
                {
                    var firstCollectionDate = DaysTable.First().GetDate();
                    var lastCollectionDate = DaysTable.Last().GetDate();

                    if (firstCollectionDate > startDate)
                    {
                        DaysTable.InsertRange(0, GetDayDescriptions(startDate, firstCollectionDate.AddDays(-1)));
                    }
                    if (lastCollectionDate < endDate)
                    {
                        DaysTable.InsertRange(DaysTable.Count - 1, GetDayDescriptions(lastCollectionDate.AddDays(1), endDate));
                    }
                }
            }

            return DaysTable.Where(d => d.GetDate() >= startDate && d.GetDate() <= endDate).ToArray();
        }

        private IEnumerable<DayDescription> GetDayDescriptions(DateTime startDate, DateTime endDate)
        {
            var totalDays = (endDate - startDate).TotalDays;

            var exceptions = _holidays.Where(h => HolidayType.Exceptional.Equals(h.HolidayType)).ToArray();

            var calDays = new List<DayDescription>();

            for (var i = 0; i < totalDays; i++)
            {
                var day = startDate.AddDays(i);
                //Check in exception
                var calDay = new DayDescription
                    {
                        Day = day.Day,
                        Month = day.Month,
                        Year = day.Year,
                        IsHoliday = false,
                        IsExceptionalWorkingDay = false,
                        Details = "Working Day"
                    };

                var workingException = exceptions.FirstOrDefault(e => e.IsWorkingException(day));
                var holiday = _holidays.FirstOrDefault(h => h.IsHoliday(day));
                if (workingException != null)
                {
                    calDay.IsExceptionalWorkingDay = true;
                    calDay.Details = workingException.Description;
                }
                else if (holiday != null)
                {
                    calDay.IsHoliday = true;
                    calDay.Details = holiday.Description;
                }
                calDays.Add(calDay);
            }
            return calDays.ToArray();
        }
    }
}