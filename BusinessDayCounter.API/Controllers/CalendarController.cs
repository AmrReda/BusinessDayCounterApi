using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BusinessDayCounter.API.ViewModels;
using BusinessDayCounter.Services;
using BusinessDayCounter.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BusinessDayCounter.API.Controllers
{
    [ApiController]
    public class CalendarController : Controller
    {
        private readonly BusinessDayCounterService _businessDayCounterService;
        private readonly IPublicHolidayProvider _publicHolidaysProvider;
        private readonly ILogger<CalendarController> _logger;

        public CalendarController(ILogger<CalendarController> logger,
            BusinessDayCounterService businessDayCounterService, IPublicHolidayProvider publicHolidaysProvider)
        {
            _logger = logger;
            _businessDayCounterService = businessDayCounterService;
            _publicHolidaysProvider = publicHolidaysProvider;
        }

        [HttpGet("/calculate")]
        [ProducesResponseType(200, Type = typeof(CalendarResponseViewModel))]
        [ProducesResponseType(400, Type = typeof(ApiException))]
        [ProducesResponseType(500, Type = typeof(ApiException))]
        public async Task<IActionResult> CalculateBusinessDaysCounter(
            [Required][FromQuery] string startDate,
            [Required][FromQuery] string endDate
        )
        {
             (DateTime firstDate, DateTime secondDate) = ParseRequestDates(startDate, endDate);
            
            var publicHolidays = await _publicHolidaysProvider.QueryPublicHolidaysAsync();
            var result = _businessDayCounterService.BusinessDaysBetweenTwoDates(
                firstDate,
                secondDate,
                publicHolidays);

            return Ok(new CalendarResponseViewModel()
            {
                NumberOfBusinessDays = result
            });
        }


        private (DateTime, DateTime) ParseRequestDates(string startDate, string endDate)
        {
            if (!DateTime.TryParse(startDate, null, System.Globalization.DateTimeStyles.RoundtripKind, out var firstDate))
            {
                throw new  ApiException(400, "incorrect startDate format");
            }

            if (!DateTime.TryParse(endDate, null, System.Globalization.DateTimeStyles.RoundtripKind, out var secondDate))
            {
                throw new  ApiException(400, "incorrect endDate format");
            }

            return (firstDate, secondDate);
        }
    }
}