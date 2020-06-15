using System;

namespace BusinessDayCounter.Services.Models
{
    public class FollowingWeekdayPublicHoliday : IPublicHoliday
    {
        private readonly int _day;
        private readonly int _month;

        public FollowingWeekdayPublicHoliday(int month, int day)
        {
            _day = day;
            _month = month;
        }

        public DateTime GetDate(int year)
        {
            if (year < DateTime.MinValue.Year)
                throw new ArgumentOutOfRangeException("year", "value for year is below the minimum value");

            
            DateTime holidayDate = new DateTime(year, _month, _day);

            DayOfWeek dayOfWeek = holidayDate.DayOfWeek;
            if (dayOfWeek == DayOfWeek.Sunday)
            {
                return holidayDate.AddDays(1);
            }
            if (dayOfWeek == DayOfWeek.Saturday)
            {
                return holidayDate.AddDays(2);
            }
            return holidayDate;
        }
    }
}