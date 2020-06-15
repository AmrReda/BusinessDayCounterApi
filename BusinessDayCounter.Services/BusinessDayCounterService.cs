using System;
using System.Collections.Generic;
using System.Linq;
using BusinessDayCounter.Services.Helpers;
using BusinessDayCounter.Services.Models;

namespace BusinessDayCounter.Services
{
    public class BusinessDayCounterService
    {   
        /// <summary>
        /// Calculates the number of business days in between two dates.
        /// </summary>
        /// <remarks>
        /// Business days are Monday, Tuesday, Wednesday, Thursday, Friday, but excluding any dates which appear in the supplied list of public holidays.
        /// The returned count should not include either firstDate or secondDate - e.g. between Monday 07-Oct-2014 and Wednesday 09-Oct-2014 is one weekday.
        /// If secondDate is equal to or before firstDate, return 0.
        /// </remarks>
        /// <param name="firstDate">The first date.</param>
        /// <param name="secondDate">The second date.</param>
        /// <param name="publicHolidays">List of public holidays.</param>
        /// <returns>Number of business days</returns>
        public int BusinessDaysBetweenTwoDates(DateTime firstDate, DateTime secondDate, IList<DateTime> publicHolidays)
        {
            if (secondDate.Date <= firstDate.Date)
            {
                throw new ArgumentException("firstDate is earlier than or equals secondDate");
            }

            var days = GetDaysBetweenExclusive(firstDate, secondDate);
            var isHoliday = new Func<DateTime, bool>(day => publicHolidays.Any(holiday => day.Date == holiday.Date));
            return days.Count(day => day.IsBusinessDay() && !isHoliday(day));
        }
        
        
        /// <summary>
        /// Calculates the number of business days in between two dates.
        /// </summary>
        /// <param name="firstDate">The first date.</param>
        /// <param name="secondDate">The second date.</param>
        /// <param name="publicHolidays">List of public holidays.</param>
        /// <remarks>
        /// Business days are Monday, Tuesday, Wednesday, Thursday, Friday, but excluding any dates which appear in the supplied list of public holidays.
        /// The returned count should not include either firstDate or secondDate - e.g. between Monday 07-Oct-2014 and Wednesday 09-Oct-2014 is one weekday.
        /// If secondDate is equal to or before firstDate, return 0.
        /// </remarks>
        /// <returns>Number of business days</returns>
        public int BusinessDaysBetweenTwoDates(DateTime firstDate, DateTime secondDate, IList<IPublicHoliday> publicHolidays)
        {
            if (secondDate.Date <= firstDate.Date)
            {
                throw new ArgumentException("firstDate is earlier than or equals secondDate");
            }

            var weekdayHolidayCount = Enumerable.Range(firstDate.Year, secondDate.Year - firstDate.Year + 1)
                .SelectMany(year => publicHolidays.Select(publicHoliday => publicHoliday.GetDate(year)))
                .Where(x => x > firstDate && x < secondDate)
                .Count(x => IsWeekday(x.DayOfWeek));

            return WeekdaysBetweenTwoDates(firstDate, secondDate) - weekdayHolidayCount;
        }
        
        private int WeekdaysBetweenTwoDates(DateTime firstDate, DateTime secondDate)
        {
            int daysBetween = 0;
            int weeks = 0;
            if (secondDate.DayOfWeek > firstDate.DayOfWeek)
            {
                daysBetween = secondDate.DayOfWeek - firstDate.DayOfWeek - 1;
            }
            else if (secondDate.DayOfWeek < firstDate.DayOfWeek)
            {
                int weekDayCount = 0;
                if (IsWeekday(firstDate.DayOfWeek))
                {
                    weekDayCount++;
                }
                if (IsWeekday(secondDate.DayOfWeek))
                {
                    weekDayCount++;
                }

                daysBetween = 6 + secondDate.DayOfWeek - firstDate.DayOfWeek - weekDayCount;
                weeks = -1;
            }

            var firstWeekStarting = firstDate.AddDays(- (int) firstDate.DayOfWeek);
            var secondWeekStarting = secondDate.AddDays(- (int) secondDate.DayOfWeek);
            weeks += (int) (secondWeekStarting.Date - firstWeekStarting.Date).TotalDays / 7;

            int weekdaysBetween = daysBetween + weeks * 5;

            return weekdaysBetween < 0 ? 0 : weekdaysBetween;
        }

        private IEnumerable<DateTime> GetDaysBetweenExclusive(DateTime firstDate, DateTime secondDate)
        {
            if (secondDate.Date <= firstDate.Date)
                yield break;

            var startDate = firstDate.Date.AddDays(1);
            var endDate = secondDate.Date.AddDays(-1);

            for (var rollingDate = startDate; rollingDate <= endDate; rollingDate = rollingDate.AddDays(1))
            {
                yield return rollingDate.Date;
            }
        }
        
        private bool IsWeekday(DayOfWeek dayOfWeek)
        {
            return dayOfWeek >= DayOfWeek.Monday && dayOfWeek <= DayOfWeek.Friday;
        }
    }
}