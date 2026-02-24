using CouponApp.Application.DTOs.Offers;
using CouponApp.Application.Exceptions;
using CouponApp.Application.Interfaces;
using CouponApp.Application.Interfaces.Repositories;
using CouponApp.Application.Interfaces.Sercives.Auth;
using CouponApp.Application.Interfaces.Sercives.Offer;
using CouponApp.Domain.Enums;
using Mapster;

namespace CouponApp.Application.Services.Offers
{
    public class AdminOfferService : IAdminOfferService
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthorizationService _authorization;

        public AdminOfferService(
            IOfferRepository offerRepository,
            IUnitOfWork unitOfWork,
            IAuthorizationService authorization)
        {
            _offerRepository = offerRepository;
            _unitOfWork = unitOfWork;
            _authorization = authorization;
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

        public async Task<IReadOnlyList<OfferResponse>> GetPendingAsync(CancellationToken cancellationToken)
        {
            _authorization.EnsureRole(UserRole.Admin);

            return await _offerRepository.GetPendingAsync(cancellationToken);
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
