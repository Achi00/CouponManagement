using CouponApp.Application.DTOs.Merchant;
using CouponApp.Application.Exceptions;
using CouponApp.Application.Interfaces;
using CouponApp.Application.Interfaces.Repositories;
using CouponApp.Application.Interfaces.Sercives;
using CouponApp.Application.Interfaces.Sercives.Auth;
using CouponApp.Domain.Entity;
using CouponApp.Domain.Enums;
using Mapster;
using System.Net;

namespace CouponApp.Application.Services
{
    public class MerchantService : IMerchantService
    {
        private readonly IMerchantRepository _merchantRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOfferRepository _offerRepository;
        private readonly ICouponRepository _couponRepository;
        private readonly IAuthorizationService _authorization;
        private readonly ICurrentUserService _currentUser;
        private readonly IUnitOfWork _unitOfWork;

        public MerchantService(
            IMerchantRepository merchantRepository,
            IAuthorizationService authorization,
            ICouponRepository couponRepository,
            IUserRepository userRepository,
            IOfferRepository offerRepository,
            ICurrentUserService currentUser,
            IUnitOfWork unitOfWork)
        {
            _merchantRepository = merchantRepository;
            _authorization = authorization;
            _couponRepository = couponRepository;
            _userRepository = userRepository;
            _offerRepository = offerRepository;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }

        public async Task<MerchantProfileResponse> GetByUserIdAsync(CancellationToken cancellationToken)
        {
            _authorization.EnsureRole(UserRole.Merchant);

            var userId = _currentUser.UserId!.Value;
            var merchant = await _merchantRepository.GetByUserIdAsync(userId, cancellationToken);

            if (merchant == null)
            {
                throw new NotFoundException("Merchant not found");
            }

            return merchant.Adapt<MerchantProfileResponse>();
        }

        public async Task RegisterAsMerchantAsync(RegisterAsMerchantRequest request, CancellationToken cancellationToken)
        {
            _authorization.EnsureAuthenticated();

            var userId = _currentUser.UserId!.Value;

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            if (user.Role == UserRole.Merchant)
            {
                throw new BusinessException("User already registered as merchant");
            }

            await _userRepository.UpdateRoleAsync(userId, UserRole.Merchant, cancellationToken);


            // create merchant entity
            var merchant = new Merchant
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                BusinessName = request.BusinessName,
                Description = request.Description
            };

            _merchantRepository.Add(merchant);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<MerchantDashboardResponse> GetDashboardAsync(CancellationToken cancellationToken)
        {
            _authorization.EnsureRole(UserRole.Merchant);

            var userId = _currentUser.UserId!.Value;

            var merchant = await _merchantRepository.GetByUserIdAsync(userId, cancellationToken);

            if (merchant == null)
            {
                throw new NotFoundException("Merchant not found");
            }

            var activeOffers = await _offerRepository.GetActiveCountByMerchantIdAsync(merchant.Id, cancellationToken);
            var totalOffers = await _offerRepository.GetTotalCountByMerchantIdAsync(merchant.Id, cancellationToken);
            var totalSold = await _couponRepository.GetSoldCountByMerchantIdAsync(merchant.Id, cancellationToken);
            var totalRevenue = await _couponRepository.GetTotalRevenueByMerchantIdAsync(merchant.Id, cancellationToken);
            var recentSales = await _couponRepository.GetSalesByMerchantIdAsync(merchant.Id, cancellationToken);

            return new MerchantDashboardResponse
            {
                ActiveOffers = activeOffers,
                TotalOffers = totalOffers,
                TotalCouponsSold = totalSold,
                TotalRevenue = totalRevenue,
                RecentSales = recentSales
            };
        }

        public async Task<List<MerchantSaleResponse>> GetSalesHistoryAsync(CancellationToken cancellationToken)
        {
            _authorization.EnsureRole(UserRole.Merchant);

            var userId = _currentUser.UserId!.Value;

            var merchant = await _merchantRepository.GetByUserIdAsync(userId, cancellationToken);
            if (merchant == null)
            {
                throw new NotFoundException("Merchant not found");
            }

            return await _couponRepository.GetSalesByMerchantIdAsync(merchant.Id, cancellationToken);
        }

        public async Task UpdateAsync(UpdateProfileMerchantRequest request, CancellationToken cancellationToken)
        {
            _authorization.EnsureRole(UserRole.Merchant);
            var userId = _currentUser.UserId!.Value;

            var merchant = await _merchantRepository.GetForUpdateByUserIdAsync(userId, cancellationToken);
            if (merchant == null)
            {
                throw new NotFoundException("Merchant not found");
            }

            request.Adapt(merchant);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
