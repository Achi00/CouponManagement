using CouponApp.Application.DTOs.User;
using CouponApp.Application.Exceptions;
using CouponApp.Application.Interfaces.Repositories;
using CouponApp.Domain.Enums;
using CouponApp.Persistence.Identity;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace CouponApp.Infrastructure.Repositories.User
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            return user.Adapt<UserResponse>();
        }

        public async Task BlockAsync(Guid id, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            user.IsBlocked = true;
            await _userManager.UpdateAsync(user);
        }

        public async Task UnblockAsync(Guid id, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            if (!user.IsBlocked)
            {
                throw new BusinessException("User is not blocked");
            }

            user.IsBlocked = false;
            await _userManager.UpdateAsync(user);
        }

        public async Task UpdateRoleAsync(Guid id, UserRole role, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            user.Role = role;
            await _userManager.UpdateAsync(user);
        }
    }
}
