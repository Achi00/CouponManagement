using CouponApp.Application.DTOs.Reservations;

namespace CouponApp.Application.Interfaces.Sercives
{
    public interface IReservationService
    {
        // checks coupon availability, deducts from offer count, sets expiry from SystemSettings
        Task<ReservationResponse> CreateAsync(Guid offerId, CancellationToken cancellationToken);

        // user manually cancels — restores coupon count
        Task CancelAsync(Guid reservationId, CancellationToken cancellationToken);

        // called by background worker — uses GetExpiredActiveReservationsAsync, restores coupon counts
        Task CancelExpiredAsync(CancellationToken cancellationToken);

        Task<List<ReservationResponse>> GetActiveByUserAsync(CancellationToken cancellationToken);
    }
}
