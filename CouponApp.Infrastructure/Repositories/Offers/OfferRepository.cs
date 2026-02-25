using CouponApp.Application.DTOs.Admin;
using CouponApp.Application.DTOs.Offers;
using CouponApp.Application.DTOs.Search;
using CouponApp.Application.Interfaces.Repositories;
using CouponApp.Domain.Entity;
using CouponApp.Domain.Enums;
using CouponApp.Persistence.Contexts;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CouponApp.Infrastructure.Repositories.Offers
{
    public class OfferRepository : IOfferRepository
    {
        private readonly DiscountManagementContext _context;
        private readonly TypeAdapterConfig _config;

        public OfferRepository(DiscountManagementContext context, TypeAdapterConfig config)
        {
            _context = context;
            _config = config;
        }
        public void Add(Offer entity)
        {
            _context.Offers.Add(entity);
        }

        public void Delete(Offer entity)
        {
            _context.Offers.Remove(entity);
        }

        public async Task<List<OfferResponse>> GetApprovedAsync(OfferFilterQuery filter, CancellationToken cancellationToken)
        {
            var query = _context.Offers
                .Include(o => o.Category)
                .AsNoTracking()
                .Where(o => o.Status == OfferStatus.Approved);

            if (!string.IsNullOrWhiteSpace(filter.SearchQuery))
                query = query.Where(o => o.Title.Contains(filter.SearchQuery) ||
                                          o.Description.Contains(filter.SearchQuery));

            if (filter.SelectedCategoryId.HasValue)
                query = query.Where(o => o.CategoryId == filter.SelectedCategoryId.Value);

            return await query
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

        public async Task<List<AdminOfferResponse>> GetPendingAsync(CancellationToken cancellationToken)
        {
            return await _context.Offers
                .AsNoTracking()
                .Where(o => o.Status == OfferStatus.Pending)
                .ProjectToType<AdminOfferResponse>(_config)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<bool> ExistsByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken)
        {
            return await _context.Offers.AsNoTracking().AnyAsync(o => o.CategoryId == categoryId, cancellationToken).ConfigureAwait(false);
        }
    }
}
