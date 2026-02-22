using CouponApp.Application.DTOs.Offers;
using CouponApp.Domain.Entity;

namespace CouponApp.Application.Interfaces.Repositories
{
    public interface IOfferRepository : IBaseRepository<Offer>
    {
        Task<List<OfferResponse>> GetApprovedAsync(CancellationToken cancellationToken);
        Task<List<OfferResponse>> GetPendingAsync(CancellationToken cancellationToken);
        Task<List<OfferResponse>> GetByMerchantIdAsync(Guid merchantId, CancellationToken cancellationToken);
    }
}
