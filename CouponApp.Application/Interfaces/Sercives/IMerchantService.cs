using CouponApp.Application.DTOs.Merchant;

namespace CouponApp.Application.Interfaces.Sercives
{
    public interface IMerchantService
    {
        // Dashboard
        // returns count of active/expired offers, total coupons sold, revenue etc.
        Task<MerchantDashboardResponse> GetDashboardAsync(CancellationToken cancellationToken);

        // Sales History
        // returns username, date, coupon code for each sale under this merchant's offers
        Task<List<MerchantSaleResponse>> GetSalesHistoryAsync(CancellationToken cancellationToken);

        // Merchant profile
        Task<MerchantProfileResponse> GetByUserIdAsync(CancellationToken cancellationToken);
        Task UpdateAsync(UpdateProfileMerchantRequest request, CancellationToken cancellationToken);

        Task<bool> MerchantProfileExistsAsync(CancellationToken cancellationToken);

        Task RegisterAsMerchantAsync(RegisterAsMerchantRequest request, CancellationToken cancellationToken);
    }
}
