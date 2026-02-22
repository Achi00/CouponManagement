using CouponApp.Application.DTOs.Merchant;

namespace CouponApp.Application.Interfaces.Sercives
{
    public interface IMerchantService
    {
        // Dashboard
        // returns count of active/expired offers, total coupons sold, revenue etc.
        Task<MerchantDashboardResponse> GetDashboardAsync(Guid userId, CancellationToken cancellationToken);

        // Sales History
        // returns username, date, coupon code for each sale under this merchant's offers
        Task<List<MerchantSaleResponse>> GetSalesHistoryAsync(Guid userId, CancellationToken cancellationToken);

        // Merchant profile
        Task<MerchantProfileResponse> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task UpdateAsync(Guid userId, UpdateProfileMerchantRequest request, CancellationToken cancellationToken);
    }
}
