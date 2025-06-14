using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MySapProject.WebApi.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            var method = context.Request.Method;
            var path = context.Request.Path;

            _logger.LogInformation("Request boshlandi: {Method} {Path}", method, path);

            await _next(context);

            stopwatch.Stop();

            var statusCode = context.Response.StatusCode;

            _logger.LogInformation("Request tugadi: {Method} {Path} status: {StatusCode} vaqt(ms): {ElapsedMilliseconds}",
                method, path, statusCode, stopwatch.ElapsedMilliseconds);
        }
    }
}
