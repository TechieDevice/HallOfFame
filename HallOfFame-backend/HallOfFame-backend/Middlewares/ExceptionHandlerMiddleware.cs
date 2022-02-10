using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using HallOfFame_backend.Logger;
using System.IO;

namespace HallOfFame_backend.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private AppSettings.AppSettings _settings;
        private ILogger _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, IOptions<AppSettings.AppSettings> options)
        {
            _next = next;
            _settings = options.Value;

            var path = _settings.Logging.LogPath.PathToFile;
            if (_settings.Logging.LogPath.CorrentDirectory) path = Path.Combine(Directory.GetCurrentDirectory(), path);

            var loggerFactory = LoggerFactory.Create(builder => builder.ClearProviders());
            loggerFactory.AddFile(path);
            _logger = loggerFactory.CreateLogger(options.Value.Logging.LogPath.LoggerName);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                _logger.LogInformation("app start");
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
