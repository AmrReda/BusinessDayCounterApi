using System;

namespace BusinessDayCounter.Services.Models
{
    public class DateHoliday : IPublicHoliday
    {
        private readonly int _day;
        private readonly int _month;

        public DateHoliday(int month, int day)
        {
            _day = day;
            _month = month;
        }

        public DateTime GetDate(int year)
        {
            if (year < DateTime.MinValue.Year)
                throw new ArgumentOutOfRangeException("year", "value for year is below the minimum value");

            DateTime holidayDate = new DateTime(year, _month, _day);
            return holidayDate;
        }
    }
}