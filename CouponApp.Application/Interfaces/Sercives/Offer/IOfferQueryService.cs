using CouponApp.Application.DTOs.Offers;
using CouponApp.Application.DTOs.Search;

namespace CouponApp.Application.Interfaces.Sercives.Offer
{
    public interface IOfferQueryService
    {
        Task<IReadOnlyList<OfferResponse>> GetApprovedAsync(OfferFilterQuery filter, CancellationToken ct);

        Task<OfferDetailsResponse> GetDetailsAsync(Guid offerId, CancellationToken ct);
    }
}
