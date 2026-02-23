using CouponApp.Application.DTOs.Coupons;
using CouponApp.Application.DTOs.Reservations;
using CouponApp.Application.DTOs.User;

namespace CouponApp.Application.Interfaces.Sercives
{
    public interface IUserProfileService
    {
        Task<UserProfileResponse> GetProfileAsync(CancellationToken cancellationToken);
        Task<List<CouponResponse>> GetMyCouponsAsync(CancellationToken cancellationToken);
        Task<List<ReservationResponse>> GetMyReservationsAsync(CancellationToken cancellationToken);
    }
}
