using Microsoft.EntityFrameworkCore;
using CouponApp.Application.Interfaces.Repositories;
using CouponApp.Domain.Entity;
using CouponApp.Persistence.Contexts;

namespace CouponApp.Infrastructure.Repositories.SystemSettings
{
    public class SystemSettingsRepository : ISystemSettingsRepository
    {
        private readonly DiscountManagementContext _context;

        public SystemSettingsRepository(DiscountManagementContext context)
        {
            _context = context;
        }
        public async Task<SystemSetting?> GetAsync(CancellationToken cancellationToken)
        {
            // tracking needed for update data
            return await _context.SystemSettings.FirstOrDefaultAsync(cancellationToken);
        }
    }
}
