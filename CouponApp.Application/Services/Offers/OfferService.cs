using CouponApp.Application.DTOs.Offers;
using CouponApp.Application.Exceptions;
using CouponApp.Application.Interfaces;
using CouponApp.Application.Interfaces.Repositories;
using CouponApp.Application.Interfaces.Sercives.Auth;
using CouponApp.Application.Interfaces.Sercives.Offer;
using CouponApp.Domain.Entity;
using CouponApp.Domain.Enums;
using Mapster;
using System.Net;
using System.Threading;

namespace CouponApp.Application.Services.Offers
{
    public class OfferService : IOfferQueryService, IMerchantOfferService
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IMerchantRepository _merchantRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly ISystemSettingsRepository _systemSettingsRepository;
        private readonly IAuthorizationService _authorization;
        private readonly IUnitOfWork _unitOfWork;
        public OfferService(
             IOfferRepository offerRepository,
            ICurrentUserService currentUser,
            IUnitOfWork unitOfWork,
            ISystemSettingsRepository systemSettingsRepository,
            IAuthorizationService authorization,
            IMerchantRepository merchantRepository)
        {
            _offerRepository = offerRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _systemSettingsRepository = systemSettingsRepository;
            _merchantRepository = merchantRepository;
            _authorization = authorization;
        }
        public async Task CreateAsync(CreateOfferRequest dto, CancellationToken cancellationToken)
        {
            _authorization.EnsureRole(UserRole.Merchant);
            var userId = _currentUser.UserId!.Value;

            var merchant = await _merchantRepository.GetByUserIdAsync(userId, cancellationToken);
            if (merchant == null)
            {
                throw new NotFoundException("Merchant not found");
            }

            var offer = dto.Adapt<Offer>();
            offer.CreatedAt = DateTime.UtcNow;
            offer.Status = OfferStatus.Pending;
            offer.MerchantId = merchant.Id;

            _offerRepository.Add(offer);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid offerId, CancellationToken cancellationToken)
        {
            _authorization.EnsureRole(UserRole.Merchant);
            var userId = _currentUser.UserId!.Value;

            // check if merchant exists
            var merchant = await _merchantRepository.GetByUserIdAsync(userId, cancellationToken);
            if (merchant == null)
            {
                throw new NotFoundException("Merchant not found");
            }
            // check if offer exists
            var offer = await _offerRepository.GetForUpdateAsync(offerId, cancellationToken);
            if (offer == null)
            {
                throw new NotFoundException("Offer not found");
            }

            // check if merchant owns offer
            if (offer.MerchantId != merchant.Id)
            {
                throw new ForbiddenException($"Merchant is not authorized to delete this offer");
            }

            _offerRepository.Delete(offer);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<OfferResponse>> GetApprovedAsync(CancellationToken cancellationToken)
        {
            return await _offerRepository.GetApprovedAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<OfferResponse>> GetByMerchantAsync(CancellationToken cancellationToken)
        {
            _authorization.EnsureRole(UserRole.Merchant);

            var userId = _currentUser.UserId!.Value;

            var merchant = await _merchantRepository.GetByUserIdAsync(userId, cancellationToken);

            if (merchant == null)
            {
                throw new MerchantProfileMissingException();
            }

            return await _offerRepository.GetByMerchantIdAsync(merchant.Id, cancellationToken);
        }

        public async Task<List<OfferResponse>> GetByMerchantsAsync(CancellationToken cancellationToken)
        {
            _authorization.EnsureRole(UserRole.Merchant);

            var userId = _currentUser.UserId!.Value;

            var merchant = await _merchantRepository.GetByUserIdAsync(userId, cancellationToken);

            if (merchant == null)
            {
                throw new MerchantProfileMissingException();
            }

            return await _offerRepository.GetByMerchantIdAsync(merchant.Id, cancellationToken);
        }

        public async Task<OfferDetailsResponse> GetDetailsAsync(Guid offerId, CancellationToken cancellationToken)
        {
            var offer = await _offerRepository.GetByIdAsync(offerId, cancellationToken);
            if (offer == null)
            {
                throw new NotFoundException("Offer not found");
            }

            return offer.Adapt<OfferDetailsResponse>();
        }

        public async Task UpdateAsync(Guid offerId, UpdateOfferRequest dto, CancellationToken cancellationToken)
        {
            _authorization.EnsureRole(UserRole.Merchant);
            var userId = _currentUser.UserId!.Value;

            // check if merchant exists
            var merchant = await _merchantRepository.GetByUserIdAsync(userId, cancellationToken);
            if (merchant == null)
            {
                throw new NotFoundException("Merchant not found");
            }
            // check if offer exists
            var offer = await _offerRepository.GetForUpdateAsync(offerId, cancellationToken);
            if (offer == null)
            {
                throw new NotFoundException("Offer not found");
            }

            // check if merchant owns offer
            if (offer.MerchantId != merchant.Id)
            {
                throw new ForbiddenException($"Merchant is not authorized to modify this offer");
            }

            // check merchant edit window
            var settings = await _systemSettingsRepository.GetAsync(cancellationToken);
            var editDeadline = offer.CreatedAt.AddHours(settings!.MerchantEditPeriodHours);

            if (DateTime.UtcNow > editDeadline)
            {
                throw new BusinessException($"Offer can no longer be edited, edit period has expired");
            }

            dto.Adapt(offer);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        
    }
}
