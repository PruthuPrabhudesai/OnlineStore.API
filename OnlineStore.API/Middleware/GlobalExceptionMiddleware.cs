using Microsoft.AspNetCore.Mvc;

namespace OnlineStore.API.Middleware
{
    public class GlobalExceptionMiddleware : IMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                throw;
            }
        }
    }
}
