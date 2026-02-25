using CouponApp.Application.Exceptions;
using CouponApp.Application.Interfaces.Logger;
using Microsoft.Extensions.Logging;

namespace CouponApp.Web.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
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
                try
                {
                    await dbLogger.LogAsync(ex, context, ex.StatusCode);
                }
                catch (Exception logEx)
                {
                    _logger.LogError(logEx, "Failed to write error to database");
                }

                _logger.LogWarning(ex, "Business exception occurred");

                var referer = context.Request.Headers["Referer"].ToString();

                if (string.IsNullOrEmpty(referer))
                {
                    referer = "/";
                }

                var uri = new Uri(referer);

                var cleanUrl = uri.GetLeftPart(UriPartial.Path);

                var redirectUrl = $"{cleanUrl}?error={Uri.EscapeDataString(ex.Message)}";

                context.Response.Redirect(redirectUrl);
            }
            catch (Exception ex)
            {
                try
                {
                    await dbLogger.LogAsync(ex, context, 500);
                }
                catch (Exception logEx)
                {
                    _logger.LogError(logEx, "Failed to write error to database");
                }
                _logger.LogError(ex, "Unhandled exception");

                context.Response.StatusCode = 500;

                context.Response.Redirect("/Error");
                _logger.LogError(ex, "Unhandled exception occurred");
            }
        }
    }
}
