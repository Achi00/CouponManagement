using CouponApp.Application.DTOs.Coupons;
using CouponApp.Domain.Entity;

namespace CouponApp.Application.Interfaces.Repositories
{
    public interface ICouponRepository : IBaseRepository<Coupon>
    {
        Task<Coupon?> GetByCodeAsync(string code, CancellationToken cancellationToken);
        Task<List<CouponResponse>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    }
}
