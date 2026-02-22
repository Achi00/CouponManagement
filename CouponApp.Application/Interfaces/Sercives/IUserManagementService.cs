using CouponApp.Application.DTOs.Merchant;
using CouponApp.Application.DTOs.User;

namespace CouponApp.Application.Interfaces.Sercives
{
    public interface IUserManagementService
    {
        Task<List<UserResponse>> GetAllUsersAsync(CancellationToken cancellationToken);
        Task<List<UserResponse>> GetAllMerchantsAsync(CancellationToken cancellationToken);
        Task<UserResponse> GetByIdAsync(Guid userId, CancellationToken cancellationToken);

        Task CreateMerchantAsync(RegisterAsMerchantRequest request, CancellationToken cancellationToken);
        // creates ApplicationUser + Merchant entity

        //Task UpdateAsync(Guid userId, UpdateUserRequest request, CancellationToken cancellationToken);
        Task BlockAsync(Guid userId, CancellationToken cancellationToken);
        Task UnblockAsync(Guid userId, CancellationToken cancellationToken);
        //Task DeleteAsync(Guid userId, CancellationToken cancellationToken);S
    }
}
