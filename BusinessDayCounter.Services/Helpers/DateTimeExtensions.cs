using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BusinessDayCounter.Services.Helpers
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// List of weekend days, for extensibility and maintainability, we will put these in a static variable.
        /// </summary>
        private static readonly DayOfWeek[] WeekendDays = {DayOfWeek.Saturday, DayOfWeek.Sunday};

        /// <summary>
        /// check if a day is a weekend
        /// </summary>
        /// <param name="date">the date to check</param>
        /// <returns>True if the given date is a weekend, otherwise False</returns>
        public static bool IsWeekend(this DateTime date)
        {
            return WeekendDays.Any(weekendDay => weekendDay == date.DayOfWeek);
        }

        /// <summary>
        /// check if a day is a business day (not a weekend day)
        /// </summary>
        /// <param name="date">the date to check</param>
        /// <returns>True if the given date is a business day, otherwise False</returns>
        public static bool IsBusinessDay(this DateTime date)
        {
            return (date.IsWeekend() == false);
        }

        public static DateTime FirstDayOfTheMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        public static DateTime LastDayOfTheMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
        }

        /// <summary>
        /// Get a list of dates in a month
        /// </summary>
        public static IEnumerable<DateTime> AllDaysInTheMonth(this DateTime date)
        {
            var firstOfMonth = date.FirstDayOfTheMonth();
            var lastOfMonth = date.LastDayOfTheMonth();

            for (var rollingDate = firstOfMonth; rollingDate <= lastOfMonth; rollingDate = rollingDate.AddDays(1))
            {
                yield return rollingDate.Date;
            }
        }

        public static int WeekOfMonth(this DateTime date)
        {
            return (date.Day - 1) / 7 + 1;
        }

        public static int WeekOfYear(this DateTime date)
        {
            return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
        }
    }
}