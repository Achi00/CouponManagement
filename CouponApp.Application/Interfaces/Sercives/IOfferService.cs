using CouponApp.Application.DTOs.Offers;

namespace CouponApp.Application.Interfaces.Sercives
{
    public interface IOfferService
    {
        // Public browsing
        Task<List<OfferResponse>> GetApprovedAsync(CancellationToken cancellationToken);
        // returns full info, remaining coupons, deadlines, and whether sold out
        Task<OfferDetailsResponse> GetDetailAsync(Guid offerId, CancellationToken cancellationToken);

        // Merchant CRUD
        Task<Guid> CreateAsync(Guid userId, CreateOfferRequest request, CancellationToken cancellationToken);
        // sets status to Pending automatically

        // checks if still within merchant edit window from SettingsService, throws if expired
        Task UpdateAsync(Guid userId, Guid offerId, UpdateOfferRequest request, CancellationToken cancellationToken);

        Task DeleteAsync(Guid userId, Guid offerId, CancellationToken cancellationToken);

        Task<List<OfferResponse>> GetByMerchantAsync(Guid userId, CancellationToken cancellationToken);

        // Admin moderation
        Task<List<OfferResponse>> GetPendingAsync(CancellationToken cancellationToken);
        Task ApproveAsync(Guid offerId, CancellationToken cancellationToken);
        Task RejectAsync(Guid offerId, string reason, CancellationToken cancellationToken);
    }
}
