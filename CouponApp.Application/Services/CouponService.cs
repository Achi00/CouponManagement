using CouponApp.Application.DTOs.Coupons;
using CouponApp.Application.Exceptions;
using CouponApp.Application.Helpers;
using CouponApp.Application.Interfaces;
using CouponApp.Application.Interfaces.Repositories;
using CouponApp.Application.Interfaces.Sercives;
using CouponApp.Application.Interfaces.Sercives.Auth;
using CouponApp.Domain.Enums;
using Mapster;

namespace CouponApp.Application.Services
{
    public class CouponService : ICouponService
    {
        private readonly ICouponRepository _couponRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public CouponService(ICouponRepository couponRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _couponRepository = couponRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<CouponResponse> GetByCodeAsync(string code, CancellationToken cancellationToken)
        {
            AuthHelper.EnsureAuthenticated(_currentUser);
            AuthHelper.EnsureRole(UserRole.Customer, _currentUser);

            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentNullException("Invalid Code to search coupon");
            }

            var coupon = await _couponRepository.GetByCodeAsync(code, cancellationToken);

            if (coupon == null)
            {
                throw new NotFoundException($"Coupon with code {code} could not be found");
            }

            return coupon.Adapt<CouponResponse>();
        }

        public Task<List<CouponResponse>> GetMyAsync(Guid userId, CancellationToken cancellationToken)
        {
            AuthHelper.EnsureAuthenticated(_currentUser);
            AuthHelper.EnsureRole(UserRole.Customer, _currentUser);

            var user = 
        }

        public Task<CouponResponse> PurchaseAsync(Guid userId, Guid offerId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<CouponResponse> PurchaseFromReservationAsync(Guid userId, Guid reservationId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
