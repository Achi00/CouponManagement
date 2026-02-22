using Microsoft.EntityFrameworkCore;
using CouponApp.Application.Interfaces.Repositories;
using CouponApp.Domain.Entity;
using CouponApp.Persistence.Contexts;
using CouponApp.Application.DTOs.Coupons;
using Mapster;

namespace CouponApp.Infrastructure.Repositories.Coupons
{
    public class CouponRepository : ICouponRepository
    {
        private readonly DiscountManagementContext _context;

        public CouponRepository(DiscountManagementContext context)
        {
            _context = context;
        }
        public void Add(Coupon entity)
        {
            _context.Coupons.Add(entity);
        }

        public void Delete(Coupon entity)
        {
            _context.Coupons.Remove(entity);
        }

        public async Task<Coupon?> GetByCodeAsync(string code, CancellationToken cancellationToken)
        {
            return await _context.Coupons.AsNoTracking().FirstOrDefaultAsync(c => c.Code == code, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Coupon?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Coupons
                .AsNoTracking()
                .Include(c => c.Offer)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }
        public async Task<Coupon?> GetForUpdateAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Coupons.FirstOrDefaultAsync(c => c.Id == id, cancellationToken).ConfigureAwait(false);
        }
        public async Task<List<CouponResponse>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Coupons.AsNoTracking().Where(x => x.UserId == userId).ProjectToType<CouponResponse>().ToListAsync(cancellationToken);
        }
    }
}
