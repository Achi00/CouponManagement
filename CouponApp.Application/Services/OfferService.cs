using Mapster;
using CouponApp.Application.DTOs.Offers;
using CouponApp.Application.Exceptions;
using CouponApp.Application.Interfaces;
using CouponApp.Application.Interfaces.Repositories;
using CouponApp.Application.Interfaces.Sercives;
using CouponApp.Application.Interfaces.Sercives.Auth;
using CouponApp.Domain.Entity;
using CouponApp.Domain.Enums;

namespace CouponApp.Application.Services
{
    public class OfferService : IOfferService
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

        public async Task<OfferResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            _authorization.EnsureAuthenticated();

            var offer = await _offerRepository.GetByIdAsync(id, cancellationToken);
            if (offer == null)
            {
                throw new NotFoundException("Offer not found");
            }

            return offer.Adapt<OfferResponse>();
        }
        public async Task<List<OfferResponse>> GetApprovedAsync(CancellationToken cancellationToken)
        {
            return await _offerRepository.GetApprovedAsync(cancellationToken);
        }

        public async Task<List<OfferResponse>> GetByMerchantAsync(Guid merchantId, CancellationToken cancellationToken)
        {
            _authorization.EnsureRole(UserRole.Merchant);

            return await _offerRepository.GetByMerchantIdAsync(merchantId, cancellationToken);
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

        // for customer browsing
        public async Task<OfferDetailsResponse> GetDetailsAsync(Guid offerId, CancellationToken cancellationToken)
        {

            var offer = await _offerRepository.GetByIdAsync(offerId, cancellationToken);
            if (offer == null)
            {
                throw new NotFoundException("Offer not found");
            }

            return offer.Adapt<OfferDetailsResponse>();
        }

        // for admin
        public async Task<List<OfferResponse>> GetPendingAsync(CancellationToken cancellationToken)
        {
            _authorization.EnsureRole(UserRole.Admin);

            return await _offerRepository.GetPendingAsync(cancellationToken);
        }

        public async Task ApproveAsync(Guid offerId, CancellationToken cancellationToken)
        {
            _authorization.EnsureRole(UserRole.Admin);

            // get offew with tracking
            var offer = await _offerRepository.GetForUpdateAsync(offerId, cancellationToken);

            if (offer == null)
            {
                throw new NotFoundException("Offer not found");
            }

            offer.Status = OfferStatus.Approved;

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<RejectOfferResponse> RejectAsync(Guid offerId, string reason, CancellationToken cancellationToken)
        {
            _authorization.EnsureRole(UserRole.Admin);

            // get offew with tracking
            var offer = await _offerRepository.GetForUpdateAsync(offerId, cancellationToken);

            if (offer == null)
            {
                throw new NotFoundException("Offer was not found");
            }

            if (string.IsNullOrWhiteSpace(reason))
            {
                throw new BusinessException("Rejection reason is required");
            }

            offer.RejectionReason = reason;
            offer.Status = OfferStatus.Rejected;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return offer.Adapt<RejectOfferResponse>();
        }
    }
}
