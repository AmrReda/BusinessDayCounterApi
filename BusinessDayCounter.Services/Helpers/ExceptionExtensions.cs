using System;
using System.Net;
using BusinessDayCounter.Services.Models;
using Microsoft.Extensions.Logging;

namespace BusinessDayCounter.Services.Helpers
{
    public static class ExceptionExtensions
    {
        public static Error ToApiError(this Exception exception, bool isDevelopment)
        {
            return exception switch
            {
                ApiException apiException => apiException.ToApiError(isDevelopment),
                _ => new Error
                {
                    Status = "500",
                    Title = "Internal server error with Calender Service",
                    Detail = "There was an internal issue when processing the request with Calender Service",
                    Meta = new Meta {{"details", isDevelopment ? exception.ToString() : null}}
                }
            };
        }
        
        public static int ToStatusCode(this Exception exception)
        {
            return exception switch
            {
                ApiException apiException => apiException.StatusCode,
                _ => (int) HttpStatusCode.InternalServerError
            };
        }
        
        public static void WriteToLogger(this Exception exception, ILogger logger)
        {
            switch (exception)
            {
                case ApiException apiException:
                    apiException.WriteToLogger(logger);
                    break;
                default:
                    logger.LogError(exception, exception.Message);
                    break;
            }
        }
    }
}