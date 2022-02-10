using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using HallOfFame_backend.Logger;
using Microsoft.Extensions.Logging;
using System.IO;

namespace HallOfFame_backend.Middlewares
{
    public class AppSettingsMiddleware
    {
        private readonly RequestDelegate _next;
        private AppSettings.AppSettings _settings;
        private ILogger _logger;

        public AppSettingsMiddleware(RequestDelegate next, IOptions<AppSettings.AppSettings> options, ILoggerFactory loggerFactory)
        {
            _next = next;
            _settings = options.Value;

            var path = _settings.Logging.LogPath.PathToFile;
            if (_settings.Logging.LogPath.CorrentDirectory) path = Path.Combine(Directory.GetCurrentDirectory(), path);

            loggerFactory.AddFile(path);
            _logger = loggerFactory.CreateLogger("HOFLogger");
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next.Invoke(context);
        }
    }
}
