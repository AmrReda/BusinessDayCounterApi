using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessDayCounter.Services.Models;

namespace BusinessDayCounter.Services
{
    public interface IPublicHolidayProvider
    {
        Task<List<IPublicHoliday>> QueryPublicHolidaysAsync();
    }
}