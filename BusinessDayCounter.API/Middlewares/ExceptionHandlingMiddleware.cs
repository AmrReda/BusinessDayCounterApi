using System;
using System.Threading.Tasks;
using BusinessDayCounter.Services.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;

namespace BusinessDayCounter.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ExceptionHandlingMiddleware(
            RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IWebHostEnvironment hostingEnvironment)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _hostingEnvironment = hostingEnvironment;
            _logger = logger ?? new NullLogger<ExceptionHandlingMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                if (context.Response.HasStarted)
                {
                    _logger.LogWarning(
                        "The response has already started, the http status code middleware will not be executed.");
                    throw;
                }

                var json = JsonConvert.SerializeObject(
                    new[] {exception.ToApiError(_hostingEnvironment.IsDevelopment())}
                );

                context.Response.StatusCode = exception.ToStatusCode();
                context.Response.ContentType = @"application/json; charset=utf-8";

                await context.Response.WriteAsync(json);
                exception.WriteToLogger(_logger);
            }
        }
    }
}