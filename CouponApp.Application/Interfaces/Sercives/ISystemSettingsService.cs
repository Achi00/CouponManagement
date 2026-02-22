using CouponApp.Application.DTOs.Settings;

namespace CouponApp.Application.Interfaces.Sercives
{
    public interface ISystemSettingsService
    {
        Task<SystemSettingsResponse> GetAsync(CancellationToken cancellationToken);
        // UpdateSystemSettingsRequest contains ReservationDuration (TimeSpan) and MerchantEditPeriod (TimeSpan)
        Task UpdateAsync(UpdateSystemSettingsRequest request, CancellationToken cancellationToken);
    }
}
