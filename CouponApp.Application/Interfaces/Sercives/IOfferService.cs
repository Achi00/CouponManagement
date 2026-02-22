using CouponApp.Application.DTOs.Offers;

namespace CouponApp.Application.Interfaces.Sercives
{
    public interface IOfferService
    {
        Task ApproveAsync(Guid offerId, CancellationToken cancellationToken);
        Task CreateAsync(Guid merchantId, CreateOfferRequest dto, CancellationToken cancellationToken);
        Task DeleteAsync(Guid merchantId, Guid offerId, CancellationToken cancellationToken);
        Task<OfferResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<OfferResponse>> GetByMerchantAsync(Guid merchantId, CancellationToken cancellationToken);
        Task<OfferDetailsResponse> GetDetailsAsync(Guid offerId, CancellationToken cancellationToken);
        // admin
        Task<List<OfferResponse>> GetPendingAsync(CancellationToken cancellationToken);
        Task<RejectOfferResponse> RejectAsync(Guid offerId, string? reason, CancellationToken cancellationToken);
        Task UpdateAsync(Guid merchantId, Guid offerId, UpdateOfferRequest dto, CancellationToken cancellationToken);
    }
}
