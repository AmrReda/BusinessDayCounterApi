using System;

namespace BusinessDayCounter.Services.Models
{ 
        public interface IPublicHoliday
        {
            DateTime GetDate(int year);
        } 
}