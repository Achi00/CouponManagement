using CouponApp.Application.DTOs.Merchant;
using CouponApp.Application.DTOs.User;
using CouponApp.Application.Exceptions;
using CouponApp.Application.Interfaces.Repositories;
using CouponApp.Application.Interfaces.Sercives;
using CouponApp.Application.Interfaces.Sercives.Auth;
using CouponApp.Domain.Enums;

namespace CouponApp.Application.Services
{
    // admin only management
    public class UserManagementService : IUserManagementService
    {
        private readonly IAuthorizationService _authorization;
        private readonly IUserRepository _userRepository;

        public UserManagementService(IAuthorizationService authorization, IUserRepository userRepository)
        {
            _authorization = authorization;
            _userRepository = userRepository;
        }

        public async Task<List<MerchantResponse>> GetAllMerchantsAsync(CancellationToken cancellationToken)
        {
            _authorization.EnsureRole(UserRole.Admin);
            return await _userRepository.GetAllMerchantsAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<List<UserResponse>> GetAllUsersAsync(CancellationToken cancellationToken)
        {
            _authorization.EnsureRole(UserRole.Admin);
            return await _userRepository.GetAllUsersAsync(cancellationToken);
        }

        public async Task<UserResponse> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            _authorization.EnsureRole(UserRole.Admin);
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken).ConfigureAwait(false);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            return user;
        }

        public async Task BlockAsync(Guid userId, CancellationToken cancellationToken)
        {
            _authorization.EnsureRole(UserRole.Admin);
            await _userRepository.BlockAsync(userId, cancellationToken);
        }

        public async Task UnblockAsync(Guid userId, CancellationToken cancellationToken)
        {
            _authorization.EnsureRole(UserRole.Admin);
            await _userRepository.UnblockAsync(userId, cancellationToken);
        }
    }
}
