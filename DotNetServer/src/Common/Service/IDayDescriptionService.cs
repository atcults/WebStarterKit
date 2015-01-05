using System;
using Common.Base;

namespace Common.Service
{
    public interface IDayDescriptionService
    {
        void SetHolidays(Holiday[] holidays);
        DayDescription[] GetDays(DateTime startDate, DateTime endDate);
    }
}