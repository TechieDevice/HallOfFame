using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace HallOfFame_backend.Middlewares
{
    public class AppSettingsMiddleware
    {
        private readonly RequestDelegate _next;
        private AppSettings _settings;

        public AppSettingsMiddleware(RequestDelegate next, IOptions<AppSettings> options)
        {
            _next = next;
            _settings = options.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next.Invoke(context);
        }
    }
}
