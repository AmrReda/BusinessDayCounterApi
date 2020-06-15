using System;
using BusinessDayCounter.Services.Models;
using Xunit;

namespace BusinessDayCounter.Unit.Tests.Holidays
{
    public class FollowingWeekdayPublicHolidayTest
    {
        [Fact]
        public void Should_GetDate_Return_Calculated_Holiday_Date_When_For_Given_Year_When_Match_Weekend()
        {
           
            var followingWeekDayPublicHoliday = new FollowingWeekdayPublicHoliday(1, 1); //New Year's eve

            var result = followingWeekDayPublicHoliday.GetDate(2011);

            Assert.Equal(new DateTime(2011, 1, 3), result);
        }
        
        [Fact]
        public void Should_GetDate_Throw_Exception_When_Given_Year_Is_Below_Min_Value()
        {
            var dayInMonthHoliday =
                new DayInMonthHoliday(6, DayOfWeek.Monday,
                    2);

            Assert.Throws<ArgumentOutOfRangeException>(() => dayInMonthHoliday.GetDate(DateTime.MinValue.Year - 1));
        }
    }
}