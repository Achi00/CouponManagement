using CouponApp.Application.Interfaces.Sercives;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CouponApp.Infrastructure.BackgroundJobs
{
    public class ExpiredReservationWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ExpiredReservationWorker> _logger;

        public ExpiredReservationWorker(IServiceScopeFactory scopeFactory, ILogger<ExpiredReservationWorker> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var reservationService = scope.ServiceProvider.GetRequiredService<IReservationService>();
                    await reservationService.CancelExpiredAsync(stoppingToken);
                    _logger.LogInformation("Expired reservations cleaned up at {Time}", DateTime.UtcNow);
                }
                catch (OperationCanceledException)
                {
                    // shutdown requested, exit here
                    break; 
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error cancelling expired reservations");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
