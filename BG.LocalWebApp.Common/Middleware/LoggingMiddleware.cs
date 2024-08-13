using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BG.LocalWebApp.Common.Middleware
{
    /// <summary>
    /// Middleware for logging the details of incoming HTTP requests and the time taken to handle them.
    /// </summary>
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the request pipeline.</param>
        /// <param name="logger">The logger instance to log request details.</param>
        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Invokes the middleware to log the request details and the time taken to process it.
        /// </summary>
        /// <param name="context">The HTTP context representing the current request.</param>
        /// <returns>A task that represents the completion of request processing.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            _logger.LogInformation("Handling request: {RequestPath}", context.Request.Path);

            await _next(context);

            stopwatch.Stop();
            _logger.LogInformation("Finished handling request: {RequestPath} in {ElapsedMilliseconds}ms", context.Request.Path, stopwatch.ElapsedMilliseconds);
        }
    }
}
