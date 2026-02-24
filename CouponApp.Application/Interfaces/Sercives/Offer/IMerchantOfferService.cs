using CouponApp.Application.DTOs.Offers;

namespace CouponApp.Application.Interfaces.Sercives.Offer
{
    public interface IMerchantOfferService
    {
        Task CreateAsync(CreateOfferRequest dto, CancellationToken ct);

        Task UpdateAsync(Guid offerId, UpdateOfferRequest dto, CancellationToken ct);

        Task DeleteAsync(Guid offerId, CancellationToken ct);

        Task<IReadOnlyList<OfferResponse>> GetByMerchantAsync(CancellationToken ct);
    }
}
