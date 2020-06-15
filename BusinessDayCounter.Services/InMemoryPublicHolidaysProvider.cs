using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessDayCounter.Services.Models;

namespace BusinessDayCounter.Services
{
    public class InMemoryPublicHolidaysProvider : IPublicHolidayProvider
    {
        public async Task<List<IPublicHoliday>> QueryPublicHolidaysAsync()
        {
            return  await Task.FromResult(new List<IPublicHoliday>
            {
                new DateHoliday(12, 25), //Christmas Day
                new DateHoliday(1, 1), //New years Eve
                new DateHoliday(12, 26), //Boxing Day
                new DayInMonthHoliday(6, DayOfWeek.Monday, 2), //Queen's Birthday
                new FollowingWeekdayPublicHoliday(1, 26), //Australia Day
                new FollowingWeekdayPublicHoliday(4, 25) //Anzac Day
            });
        }
    }
}