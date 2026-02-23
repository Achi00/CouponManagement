using CouponApp.Application.DTOs.Merchant;
using CouponApp.Application.DTOs.User;
using CouponApp.Application.Exceptions;
using CouponApp.Application.Interfaces.Repositories;
using CouponApp.Domain.Enums;
using CouponApp.Persistence.Contexts;
using CouponApp.Persistence.Identity;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CouponApp.Infrastructure.Repositories.User
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DiscountManagementContext _context;

        public UserRepository(UserManager<ApplicationUser> userManager, DiscountManagementContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<List<UserResponse>> GetAllUsersAsync(CancellationToken cancellationToken)
        {
            return await _userManager.Users.AsNoTracking().ProjectToType<UserResponse>().ToListAsync(cancellationToken).ConfigureAwait(false);
        }
        public async Task<List<MerchantResponse>> GetAllMerchantsAsync(CancellationToken cancellationToken)
        {
            return await _context.Merchants
                .AsNoTracking()
                .Join(_context.Users,
                    m => m.UserId,
                    u => u.Id,
                    (m, u) => new MerchantResponse
                    {
                        Id = m.Id,
                        UserId = u.Id,
                        Username = u.UserName,
                        Email = u.Email,
                        BusinessName = m.BusinessName,
                        Description = m.Description,
                        IsBlocked = u.IsBlocked
                    })
                .ToListAsync(cancellationToken);
        }

        public async Task<UserResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(id.ToString()).ConfigureAwait(false);

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            return user.Adapt<UserResponse>();
        }

        public async Task BlockAsync(Guid id, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(id.ToString()).ConfigureAwait(false);

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            user.IsBlocked = true;
            await _userManager.UpdateAsync(user).ConfigureAwait(false);
        }

        public async Task UnblockAsync(Guid id, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(id.ToString()).ConfigureAwait(false);

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            if (!user.IsBlocked)
            {
                throw new BusinessException("User is not blocked");
            }

            user.IsBlocked = false;
            await _userManager.UpdateAsync(user).ConfigureAwait(false);
        }

        public async Task UpdateRoleAsync(Guid id, UserRole role, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(id.ToString()).ConfigureAwait(false);

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            user.Role = role;
            await _userManager.UpdateAsync(user).ConfigureAwait(false);
        }
    }
}
