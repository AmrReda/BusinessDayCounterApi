using System;
using System.Collections.Generic;
using BusinessDayCounter.Services;
using BusinessDayCounter.Services.Models;
using Xunit;

namespace BusinessDayCounter.Unit.Tests
{
    public class BusinessDayCounterServiceTests
    {
        [Theory]
        [MemberData(nameof(TestData))]
        public void
            ShouldBusinessDaysBetweenTwoDates_Returns_NumbersOfDaysBetween_TwoDates_With_Given_PublicHoliday_Dates(
                DateTime firstDate,
                DateTime secondDate,
                int expected)
        {
            var businessDayCounterService = new BusinessDayCounterService();
            var publicHolidays =
                new List<DateTime>
                {
                    new DateTime(2013, 12, 25), // Christmas Day
                    new DateTime(2013, 12, 26), //Boxing Day
                    new DateTime(2014, 1, 1) // New Year's Day
                };
            var result =
                businessDayCounterService.BusinessDaysBetweenTwoDates(firstDate, secondDate, publicHolidays);


            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> TestData =>
            new List<object[]>
            {
                new object[]
                {
                    new DateTime(2013, 10, 7),
                    new DateTime(2013, 10, 9),
                    1
                },
                new object[]
                {
                    new DateTime(2013, 12, 24),
                    new DateTime(2013, 12, 27),
                    0
                },
                new object[]
                {
                    new DateTime(2013, 10, 7),
                    new DateTime(2014, 1, 1),
                    59
                },
                new object[]
                {
                    new DateTime(2014, 8, 7),
                    new DateTime(2014, 8, 11),
                    1
                },
            };


        [Fact]
        public void
            ShouldBusinessDaysBetweenTwoDates_Throw_Exception_When_Given_EndDate_Is_Later_Or_Equal_StartDate()
        {
            var businessDayCounterService = new BusinessDayCounterService();
            Assert.Throws<ArgumentException>(() => businessDayCounterService.BusinessDaysBetweenTwoDates(
                new DateTime(2014, 06, 20),
                new DateTime(2014, 06, 10),
                new List<IPublicHoliday>() { }
            ));
        }


        [Fact]
        public void
            ShouldBusinessDaysBetweenTwoDates_Returns_NumbersOfDaysBetween_TwoDates_With_Given_Certain_Occurrence_On_A_Certain_Day_In_A_Month()
        {
            var businessDayCounterService = new BusinessDayCounterService();
            var dayInMonthHoliday =
                new DayInMonthHoliday(6, DayOfWeek.Monday,
                    2); //Queen's Birthday on the second Monday of June every Year
            var result = businessDayCounterService.BusinessDaysBetweenTwoDates(
                new DateTime(2014, 06, 01),
                new DateTime(2014, 06, 23),
                new List<IPublicHoliday>() {dayInMonthHoliday}
            );

            Assert.Equal(14, result);
        }

        [Fact]
        public void
            ShouldBusinessDaysBetweenTwoDates_Returns_NumbersOfDaysBetween_TwoDates_With_Given_On_The_Same_Day_As_far_As_It_Is_not_Weekend()
        {
            var businessDayCounterService = new BusinessDayCounterService();
            var followingWeekDayPublicHoliday = new FollowingWeekdayPublicHoliday(1, 1);
            var result = businessDayCounterService.BusinessDaysBetweenTwoDates(
                new DateTime(2013, 12, 25),
                new DateTime(2014, 1, 5),
                new List<IPublicHoliday>() {followingWeekDayPublicHoliday}
            );

            Assert.Equal(6, result);
        }
    }
}