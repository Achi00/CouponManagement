using CouponApp.Application.DTOs.Offers;

namespace CouponApp.Application.Interfaces.Sercives.Offer
{
    public interface IOfferQueryService
    {
        Task<IReadOnlyList<OfferResponse>> GetApprovedAsync(CancellationToken ct);

        Task<OfferDetailsResponse> GetDetailsAsync(Guid offerId, CancellationToken ct);
    }
}
