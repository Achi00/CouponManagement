using Mapster;
using Microsoft.EntityFrameworkCore;
using CouponApp.Application.DTOs.Offers;
using CouponApp.Application.Interfaces.Repositories;
using CouponApp.Domain.Entity;
using CouponApp.Domain.Enums;
using CouponApp.Persistence.Contexts;

namespace CouponApp.Infrastructure.Repositories.Offers
{
    public class OfferRepository : IOfferRepository
    {
        private readonly DiscountManagementContext _context;

        public OfferRepository(DiscountManagementContext context)
        {
            _context = context;
        }
        public void Add(Offer entity)
        {
            _context.Offers.Add(entity);
        }

        public void Delete(Offer entity)
        {
            _context.Offers.Remove(entity);
        }

        public async Task<List<OfferResponse>> GetApprovedAsync(CancellationToken cancellationToken)
        {
            return await _context.Offers
                .Include(o => o.Category)
                .AsNoTracking()
                .Where(o => o.Status == OfferStatus.Approved)
                .ProjectToType<OfferResponse>()
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Offer?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _context.Offers
                .AsNoTracking()
                .Include(o => o.Category)
                .Include(o => o.Merchant)
                .FirstOrDefaultAsync(o => o.Id == id, ct)
                .ConfigureAwait(false);
        }

        public async Task<List<OfferResponse>> GetByMerchantIdAsync(Guid merchantId, CancellationToken cancellationToken)
        {
            return await _context.Offers
                .AsNoTracking()
                .Where(o => o.MerchantId == merchantId)
                .ProjectToType<OfferResponse>()
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<int> GetTotalCountByMerchantIdAsync(Guid merchantId, CancellationToken cancellationToken)
        {
            return await _context.Offers.CountAsync(x => x.MerchantId == merchantId, cancellationToken).ConfigureAwait(false);
        }

        public async Task<int> GetActiveCountByMerchantIdAsync(Guid merchantId, CancellationToken cancellationToken)
        {
            return await _context.Offers.CountAsync(x => x.MerchantId == merchantId && x.Status == OfferStatus.Approved, cancellationToken).ConfigureAwait(false);
        }

        public async Task<int> GetExpiredCountByMerchantIdAsync(Guid merchantId, CancellationToken cancellationToken)
        {
            return await _context.Offers.CountAsync(x => x.MerchantId == merchantId && x.Status == OfferStatus.Expired, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Offer?> GetForUpdateAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Offers.FirstOrDefaultAsync(o => o.Id == id, cancellationToken).ConfigureAwait(false);
        }

        public async Task<List<OfferResponse>> GetPendingAsync(CancellationToken cancellationToken)
        {
            return await _context.Offers
                .AsNoTracking()
                .Where(o => o.Status == OfferStatus.Pending)
                .ProjectToType<OfferResponse>()
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        
    }
}
