using System;

namespace BusinessDayCounter.Services.Models
{
    public class ApiException : Exception
    {
        public int StatusCode { get; set; }
        
        public string ContentType { get; set; } = @"application/json";
        public string Title { get; set; } 
        
        public ApiException(int statusCode)
        {
            this.StatusCode = statusCode;
        }

        public ApiException(int statusCode, string message) : base(message)
        {
            this.StatusCode = statusCode;
            this.Title = message;
        }

        public ApiException(int statusCode, Exception inner) 
            : this(statusCode, inner.ToString())
        {
        } 
    }
}