using Microsoft.EntityFrameworkCore;
using CouponApp.Application.Interfaces.Repositories;
using CouponApp.Domain.Entity;
using CouponApp.Persistence.Contexts;

namespace CouponApp.Infrastructure.Repositories.Merchants
{
    public class MerchantRepository : IMerchantRepository
    {
        private readonly DiscountManagementContext _context;

        public MerchantRepository(DiscountManagementContext context)
        {
            _context = context;
        }
        public void Add(Merchant entity)
        {
            _context.Merchants.Add(entity);
        }

        public void Delete(Merchant entity)
        {
            _context.Merchants.Remove(entity);
        }

        public async Task<Merchant?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Merchants
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Merchant?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Merchants
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Merchant?> GetForUpdateAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Merchants
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
