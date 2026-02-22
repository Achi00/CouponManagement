using CouponApp.Application.DTOs.Offers;
using CouponApp.Domain.Entity;

namespace CouponApp.Application.Interfaces.Repositories
{
    public interface IOfferRepository : IBaseRepository<Offer>
    {
        Task<List<OfferResponse>> GetApprovedAsync(CancellationToken cancellationToken);
        Task<List<OfferResponse>> GetPendingAsync(CancellationToken cancellationToken);
        Task<List<OfferResponse>> GetByMerchantIdAsync(Guid merchantId, CancellationToken cancellationToken);
        Task<int> GetActiveCountByMerchantIdAsync(Guid merchantId, CancellationToken cancellationToken);
        Task<int> GetTotalCountByMerchantIdAsync(Guid merchantId, CancellationToken cancellationToken);
        Task<int> GetExpiredCountByMerchantIdAsync(Guid merchantId, CancellationToken cancellationToken);
    }
}
