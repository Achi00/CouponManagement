using CouponApp.Application.DTOs.Coupons;

namespace CouponApp.Application.Interfaces.Sercives
{
    public interface ICouponService
    {
        // if reservation exists use it, otherwise check availability directly
        // generates unique code, deducts from offer count if no reservation
        Task<CouponResponse> PurchaseAsync(Guid userId, Guid offerId, CancellationToken cancellationToken);

        // converts existing reservation to a purchased coupon
        Task<CouponResponse> PurchaseFromReservationAsync(Guid userId, Guid reservationId, CancellationToken cancellationToken);

        // returns user's coupons with status (active/used/expired)
        Task<List<CouponResponse>> GetMyAsync(Guid userId, CancellationToken cancellationToken);

        // for merchant to look up a coupon from sales history
        Task<CouponResponse> GetByCodeAsync(string code, CancellationToken cancellationToken);
    }
}
