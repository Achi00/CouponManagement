using CouponApp.Application.DTOs.Coupons;
using CouponApp.Application.DTOs.Merchant;
using CouponApp.Application.DTOs.Reservations;
using CouponApp.Application.DTOs.User;
using CouponApp.Application.Exceptions;
using CouponApp.Application.Interfaces.Repositories;
using CouponApp.Application.Interfaces.Sercives;
using CouponApp.Application.Interfaces.Sercives.Auth;
using CouponApp.Domain.Enums;
using Mapster;

namespace CouponApp.Application.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IAuthorizationService _authorization;
        private readonly IUserRepository _userRepository;
        private readonly ICouponRepository _couponRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IMerchantRepository _merchantRepository;
        private readonly ICurrentUserService _currentUser;

        public UserProfileService(
            IAuthorizationService authorization, 
            IUserRepository userRepository, 
            ICouponRepository couponRepository, 
            IReservationRepository reservationRepository,
            IMerchantRepository merchantRepository,
            ICurrentUserService currentUser)
        {
            _authorization = authorization;
            _userRepository = userRepository;
            _couponRepository = couponRepository;
            _reservationRepository = reservationRepository;
            _merchantRepository = merchantRepository;
            _currentUser = currentUser;
        }
        public async Task<List<CouponResponse>> GetMyCouponsAsync(CancellationToken cancellationToken)
        {
            _authorization.EnsureAuthenticated();
            var userId = _currentUser.UserId!.Value;
            
            return await _couponRepository.GetByUserIdAsync(userId, cancellationToken);
        }

        public async Task<List<ReservationResponse>> GetMyReservationsAsync(CancellationToken cancellationToken)
        {
            _authorization.EnsureAuthenticated();
            var userId = _currentUser.UserId!.Value;

            var reservations = await _reservationRepository.GetActiveByUserIdAsync(userId, cancellationToken);

            return reservations.Adapt<List<ReservationResponse>>();
        }

        public async Task<UserProfileResponse> GetProfileAsync(CancellationToken cancellationToken)
        {
            _authorization.EnsureAuthenticated();
            var userId = _currentUser.UserId!.Value;

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            // check if user is merchant and get merchant data
            MerchantProfileResponse? merchant = null;
            if (user.Role == UserRole.Merchant)
            {
                var merchantEntity = await _merchantRepository.GetByUserIdAsync(userId, cancellationToken);
                merchant = merchantEntity?.Adapt<MerchantProfileResponse>();
            }

            // get stats
            var coupons = await _couponRepository.GetByUserIdAsync(userId, cancellationToken);
            var reservations = await _reservationRepository.GetActiveByUserIdAsync(userId, cancellationToken);

            return new UserProfileResponse
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Role = user.Role,
                IsBlocked = user.IsBlocked,
                Merchant = merchant,
                Stats = new UserStats
                {
                    TotalCoupons = coupons.Count,
                    ActiveCoupons = coupons.Count(c => c.Status == CouponStatus.Active),
                    ActiveReservations = reservations.Count
                }
            };
        }
    }
}
