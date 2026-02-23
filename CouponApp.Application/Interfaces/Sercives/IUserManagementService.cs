using CouponApp.Application.DTOs.Merchant;
using CouponApp.Application.DTOs.User;

namespace CouponApp.Application.Interfaces.Sercives
{
    public interface IUserManagementService
    {
        Task<List<UserResponse>> GetAllUsersAsync(CancellationToken cancellationToken);
        Task<List<MerchantResponse>> GetAllMerchantsAsync(CancellationToken cancellationToken);
        Task<UserResponse> GetByIdAsync(Guid userId, CancellationToken cancellationToken);

        //Task UpdateAsync(Guid userId, UpdateUserRequest request, CancellationToken cancellationToken);
        Task BlockAsync(Guid userId, CancellationToken cancellationToken);
        Task UnblockAsync(Guid userId, CancellationToken cancellationToken);
    }
}
