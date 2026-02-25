using CouponApp.Application.Exceptions;
using CouponApp.Application.Interfaces.Logger;
using System.Text.Json;

namespace CouponApp.API.Middlewares
{
    public class ApiExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiExceptionMiddleware> _logger;

        public ApiExceptionMiddleware(
            RequestDelegate next,
            ILogger<ApiExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, IErrorLogger dbLogger)
        {
            try
            {
                await _next(context);
            }
            catch (AppException ex)
            {
                await HandleAppException(context, ex, dbLogger);
            }
            catch (Exception ex)
            {
                await HandleUnknownException(context, ex, dbLogger);
            }
        }

        private async Task HandleAppException(
            HttpContext context,
            AppException ex,
            IErrorLogger dbLogger)
        {
            try
            {
                await dbLogger.LogAsync(ex, context, ex.StatusCode);
            }
            catch (Exception logEx)
            {
                _logger.LogError(logEx, "Error logging failed");
            }

            _logger.LogWarning(ex, "Business exception");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex.StatusCode;

            var response = new
            {
                error = ex.Message,
                status = ex.StatusCode
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private async Task HandleUnknownException(
            HttpContext context,
            Exception ex,
            IErrorLogger dbLogger)
        {
            try
            {
                await dbLogger.LogAsync(ex, context, 500);
            }
            catch (Exception logEx)
            {
                _logger.LogError(logEx, "Error logging failed");
            }

            _logger.LogError(ex, "Unhandled exception");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;

            var response = new
            {
                error = "Internal server error",
                status = 500
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
