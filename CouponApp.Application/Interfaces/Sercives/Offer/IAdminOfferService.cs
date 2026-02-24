using CouponApp.Application.DTOs.Offers;

namespace CouponApp.Application.Interfaces.Sercives.Offer
{
    public interface IAdminOfferService
    {
        Task<IReadOnlyList<OfferResponse>> GetPendingAsync(CancellationToken ct);

        Task ApproveAsync(Guid offerId, CancellationToken ct);

        Task<RejectOfferResponse> RejectAsync(Guid offerId, string reason, CancellationToken ct);
    }
}
