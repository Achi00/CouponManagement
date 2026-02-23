using CouponApp.Application.DTOs.Reservations;
using CouponApp.Application.Exceptions;
using CouponApp.Application.Interfaces;
using CouponApp.Application.Interfaces.Repositories;
using CouponApp.Application.Interfaces.Sercives;
using CouponApp.Application.Interfaces.Sercives.Auth;
using CouponApp.Domain.Entity;
using CouponApp.Domain.Enums;
using Mapster;

namespace CouponApp.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IOfferRepository _offerRepository;
        private readonly ISystemSettingsRepository _systemSettingsRepository;
        private readonly IAuthorizationService _authorization;
        private readonly ICurrentUserService _currentUser;
        private readonly IUnitOfWork _unitOfWork;

        public ReservationService(
            IReservationRepository reservationRepository,
            IOfferRepository offerRepository,
            ISystemSettingsRepository systemSettingsRepository,
            IAuthorizationService authorization,
            ICurrentUserService currentUser,
            IUnitOfWork unitOfWork
            )
        {
            _reservationRepository = reservationRepository;
            _offerRepository = offerRepository;
            _systemSettingsRepository = systemSettingsRepository;
            _authorization = authorization;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }
        public async Task<ReservationResponse> CreateAsync(Guid offerId, CancellationToken cancellationToken)
        {
            _authorization.EnsureAuthenticated();
            var userId = _currentUser.UserId!.Value;

            // check non existing active reservations for this offerid
            var activeReservation = await _reservationRepository.GetActiveByUserIdAsync(userId, cancellationToken);

            if (activeReservation.Any(r => r.OfferId == offerId))
            {
                throw new BusinessException("You already have an active reservation for this offer");
            }

            var offer = await _offerRepository.GetForUpdateAsync(offerId, cancellationToken);

            if (offer == null)
            {
                throw new NotFoundException("Offer not found");
            }
            if (offer.Status != OfferStatus.Approved)
            {
                throw new BusinessException("Offer is not available");
            }

            if (offer.RemainingCoupons <= 0)
            {
                throw new BusinessException("No coupons available");
            }

            var settings = await _systemSettingsRepository.GetAsync(cancellationToken);

            if (settings == null)
            {
                throw new BusinessException("System settings not configured");
            }

            offer.RemainingCoupons--;

            var reservation = new Reservation
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                OfferId = offerId,
                ExpiresAt = DateTime.UtcNow.AddMinutes(settings.ReservationDurationMinutes),
                Status = ReservationStatus.Active
            };

            _reservationRepository.Add(reservation);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return reservation.Adapt<ReservationResponse>();
        }

        public async Task<List<ReservationResponse>> GetActiveByUserAsync(CancellationToken cancellationToken)
        {
            _authorization.EnsureAuthenticated();
            var userId = _currentUser.UserId!.Value;

            var reservations = await _reservationRepository.GetActiveByUserIdAsync(userId, cancellationToken);
            return reservations.Adapt<List<ReservationResponse>>();
        }
        // user manualy cansels there reservation
        public async Task CancelAsync(Guid reservationId, CancellationToken cancellationToken)
        {
            _authorization.EnsureRole(UserRole.Customer);
            var userId = _currentUser.UserId!.Value;

            var reservation = await _reservationRepository.GetForUpdateAsync(reservationId, cancellationToken);
            if (reservation == null)
            {
                throw new NotFoundException("Reservation not found");
            }
            if (reservation.UserId != userId)
            {
                throw new BusinessException("This reservation does not belong to you");
            }

            var offer = await _offerRepository.GetForUpdateAsync(reservation.OfferId, cancellationToken);
            if (offer != null)
            {
                offer.RemainingCoupons++;
            }

            _reservationRepository.Delete(reservation);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        // called by background worker, no auth check needed
        public async Task CancelExpiredAsync(CancellationToken cancellationToken)
        {
            var expired = await _reservationRepository.GetExpiredActiveReservationsAsync(DateTime.UtcNow, cancellationToken);

            if (!expired.Any()) return;

            foreach (var reservation in expired)
            {
                var offer = await _offerRepository.GetForUpdateAsync(reservation.OfferId, cancellationToken);
                if (offer != null) offer.RemainingCoupons++;

                _reservationRepository.Delete(reservation);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
