using CouponApp.Application.DTOs.Coupons;
using CouponApp.Application.DTOs.Merchant;
using CouponApp.Domain.Entity;

namespace CouponApp.Application.Interfaces.Repositories
{
    public interface ICouponRepository : IBaseRepository<Coupon>
    {
        Task<Coupon?> GetByCodeAsync(string code, CancellationToken cancellationToken);
        Task<List<CouponResponse>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<List<MerchantSaleResponse>> GetSalesByMerchantIdAsync(Guid merchantId, CancellationToken cancellationToken);
        Task<int> GetSoldCountByMerchantIdAsync(Guid merchantId, CancellationToken cancellationToken);
        Task<decimal> GetTotalRevenueByMerchantIdAsync(Guid merchantId, CancellationToken cancellationToken);
    }
}
