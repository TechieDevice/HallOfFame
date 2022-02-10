using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace HallOfFame_backend.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private AppSettings _settings;

        public ExceptionHandlerMiddleware(RequestDelegate next, IOptions<AppSettings> options)
        {
            _next = next;
            _settings = options.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionMessageAsync(context, ex).ConfigureAwait(false);
            }
        }

        private static Task HandleExceptionMessageAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            int statusCode = (int)HttpStatusCode.InternalServerError;
            var result = JsonConvert.SerializeObject(new
            {
                StatusCode = statusCode,
                ErrorMessage = exception.Message
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsync(result);
        }
    }
}
