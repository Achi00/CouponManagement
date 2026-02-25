using CouponApp.Application.Interfaces.Logger;
using CouponApp.Domain.Entity;
using CouponApp.Persistence.Contexts;
using Microsoft.AspNetCore.Http;

namespace CouponApp.Persistence.Logger
{
    public class DbErrorLogger : IErrorLogger
    {
        private readonly DiscountManagementContext _context;

        public DbErrorLogger(DiscountManagementContext context)
        {
            _context = context;
        }

        public async Task LogAsync(Exception ex, HttpContext http, int statusCode)
        {
            var stack = ex.ToString();
            if (stack.Length >= 2000)
            {
                stack = stack.Substring(0, 2000);

            }

            var log = new ErrorLog
            {
                Id = Guid.NewGuid(),
                CreatedAtUtc = DateTime.UtcNow,
                Message = ex.Message,
                Exception = ex.GetType().Name,
                StackTrace = stack,
                Path = http.Request.Path,
                Method = http.Request.Method,
                StatusCode = statusCode,
                UserId = http.User?.Identity?.Name,
                IpAddress = http.Connection.RemoteIpAddress?.ToString()
            };

            _context.ErrorLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
