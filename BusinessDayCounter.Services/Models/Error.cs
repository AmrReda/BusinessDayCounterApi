using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace BusinessDayCounter.Services.Models
{
    public class Error
    {
        public string Status { get; set; }

        public string Code { get; set; }

        public string Title { get; set; }

        public string Detail { get; set; }

        public string Source { get; set; }
        public Meta Meta { get; set; }
    }
    
    public class Meta : Dictionary<string, JToken>
    {
    }
}