using System;
using BusinessDayCounter.Services.Models;
using Xunit;

namespace BusinessDayCounter.Unit.Tests.Holidays
{
    public class DateHolidayTest
    {
        [Fact]
        public void Should_GetDate_Return_Calculated_Holiday_Date_When_For_Given_Year()
        {
            var dayInMonthHoliday =
                new DateHoliday(6, 6);

            var result = dayInMonthHoliday.GetDate(2014);

            Assert.Equal(new DateTime(2014, 6, 6), result);
        }
        
        [Fact]
        public void Should_GetDate_Throw_Exception_When_Given_Year_Is_Below_Min_Value()
        {
            var dayInMonthHoliday =
                new DateHoliday(6, 6);

            Assert.Throws<ArgumentOutOfRangeException>(() => dayInMonthHoliday.GetDate(DateTime.MinValue.Year - 1));
        }
    }
}