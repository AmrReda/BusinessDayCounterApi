using System;

namespace BusinessDayCounter.Services.Models
{
    public class DayInMonthHoliday : IPublicHoliday
    {
        private readonly int _month;
        private readonly DayOfWeek _holidayDay;
        private readonly int _instance;

        public DayInMonthHoliday(int month, DayOfWeek holidayDay, int instance)
        {
            _month = month;
            _holidayDay = holidayDay;
            _instance = instance;
        }

        public DateTime GetDate(int year)
        {
            if (year < DateTime.MinValue.Year)
                throw new ArgumentOutOfRangeException("year", "value for year is below the minimum value");
            
            var firstDayOfMonth = new DateTime(year, _month, 1);
            var dayDifference = ((_holidayDay + 7) - firstDayOfMonth.DayOfWeek) % 7;
            var firstInstanceOfDay = new DateTime(year, _month, 1 + dayDifference);
            var holidayDate = firstInstanceOfDay.AddDays((_instance - 1) * 7);
            return holidayDate;
        }
    }
}