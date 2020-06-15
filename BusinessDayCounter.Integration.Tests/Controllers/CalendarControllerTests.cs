using System.Net.Http;
using System.Threading.Tasks;
using BusinessDayCounter.API.ViewModels;
using Newtonsoft.Json;
using Xunit;

namespace BusinessDayCounter.Integration.Tests.Controllers
{
    public class CalendarControllerTests : IClassFixture<BusinessDayCounterAppFactory>
    {
        private readonly HttpClient _client;

        public CalendarControllerTests(BusinessDayCounterAppFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CalculateBusinessDays_Between_Two_Days()
        {
            string startDate = "12-12-2013";
            string endDate = "10-1-2014";
            var requestUrl = $"calculate?startDate={startDate}&endDate={endDate}";
             
            var httpResponse = await _client.GetAsync(requestUrl);
            
            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var calendarResponse = JsonConvert.DeserializeObject<CalendarResponseViewModel>(stringResponse);
            
            Assert.Equal(202, calendarResponse.NumberOfBusinessDays);
        }
        
        
        //TODO: Write more integration tests
        
    }
}