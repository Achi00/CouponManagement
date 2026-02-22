using CouponApp.Application.DTOs.Merchant;

namespace CouponApp.Application.Services
{
    public interface IMerchantService
    {
        Task<MerchantProfileResponse> GetByUserIdAsync(CancellationToken cancellationToken);
        Task<MerchantDashboardResponse> GetDashboardAsync(CancellationToken cancellationToken);
        Task<List<MerchantSaleResponse>> GetSalesHistoryAsync(CancellationToken cancellationToken);
        Task RegisterAsMerchantAsync(RegisterAsMerchantRequest request, CancellationToken cancellationToken);
        Task UpdateAsync(UpdateProfileMerchantRequest request, CancellationToken cancellationToken);
    }
}