using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

namespace CouponApp.API.Middlewares
{
    public static class HealthCheckResponseWriter
    {
        public static async Task WriteResponse(HttpContext context, HealthReport report)
        {
            context.Response.ContentType = "application/json";

            var response = new
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(x => new
                {
                    name = x.Key,
                    status = x.Value.Status.ToString()
                })
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
