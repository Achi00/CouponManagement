using CouponApp.Application.DTOs.User;
using CouponApp.Domain.Enums;

namespace CouponApp.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<UserResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task UpdateRoleAsync(Guid id, UserRole role, CancellationToken cancellationToken);
        Task BlockAsync(Guid id, CancellationToken cancellationToken);
        Task UnblockAsync(Guid id, CancellationToken cancellationToken);
    }
}
