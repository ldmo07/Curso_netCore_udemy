using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Helpers.Errors
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _looger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next,ILogger<ExceptionMiddleware> looger, IHostEnvironment env)
        {
            _next = next;
            _looger = looger;
            _env = env;
        }

        public async Task InvokeAsync (HttpContext context ){
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var statusCode = (int)HttpStatusCode.InternalServerError;

                _looger.LogError(ex,ex.Message);
                context.Response.ContentType="application/json";
                context.Response.StatusCode = statusCode;

                var response = _env.IsDevelopment() 
                ? new ApiException(statusCode,ex.Message,ex.StackTrace.ToString())
                : new ApiException(statusCode);
                
                var options = new JsonSerializerOptions{
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var json = JsonSerializer.Serialize(response,options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}