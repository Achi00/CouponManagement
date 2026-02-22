using CouponApp.Application.DTOs.Coupons;
using CouponApp.Application.Exceptions;
using CouponApp.Application.Interfaces;
using CouponApp.Application.Interfaces.Repositories;
using CouponApp.Application.Interfaces.Sercives;
using CouponApp.Application.Interfaces.Sercives.Auth;
using CouponApp.Domain.Entity;
using Mapster;

namespace CouponApp.Application.Services
{
    public class CouponService : ICouponService
    {
        private readonly ICouponRepository _couponRepository;
        private readonly IOfferRepository _offerRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthorizationService _authorization;
        private readonly ICurrentUserService _currentUser;
        private readonly ICouponCodeGenerator _couponCodeGenerator;

        public CouponService(
            ICouponRepository couponRepository, 
            IOfferRepository offerRepository, 
            IReservationRepository reservationRepository, 
            IUnitOfWork unitOfWork, 
            IAuthorizationService authorization, 
            ICurrentUserService currentUser,
            ICouponCodeGenerator couponCodeGenerator)
        {
            _couponRepository = couponRepository;
            _offerRepository = offerRepository;
            _reservationRepository = reservationRepository;
            _unitOfWork = unitOfWork;
            _authorization = authorization;
            _currentUser = currentUser;
            _couponCodeGenerator = couponCodeGenerator;
        }

        public async Task<CouponResponse> GetByCodeAsync(string code, CancellationToken cancellationToken)
        {
            _authorization.EnsureAuthenticated();

            if (string.IsNullOrWhiteSpace(code))
            {
                throw new BusinessException("Invalid Code to search coupon");
            }

            var coupon = await _couponRepository.GetByCodeAsync(code, cancellationToken);

            if (coupon == null)
            {
                throw new NotFoundException($"Coupon with code {code} could not be found");
            }

            return coupon.Adapt<CouponResponse>();
        }

        public async Task<List<CouponResponse>> GetMyAsync(CancellationToken cancellationToken)
        {
            _authorization.EnsureAuthenticated();

            var userId = _currentUser.UserId!.Value;

            return await _couponRepository.GetByUserIdAsync(userId, cancellationToken);
        }

        // buy without reserving offer, coupon count not deducted yet
        public async Task<CouponResponse> PurchaseAsync(Guid offerId, CancellationToken cancellationToken)
        {
            _authorization.EnsureAuthenticated();
            var userId = _currentUser.UserId!.Value;

            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var offer = await _offerRepository.GetForUpdateAsync(offerId, cancellationToken);
                if (offer == null)
                {
                    throw new NotFoundException("Offer not found");
                }
                if (offer.RemainingCoupons <= 0)
                {
                    throw new BusinessException("No coupons available");
                }

                var activeReservations = await _reservationRepository.GetActiveByUserIdAsync(userId, cancellationToken);
                var existingReservation = activeReservations.FirstOrDefault(r => r.OfferId == offerId);

                Coupon coupon;

                if (existingReservation != null)
                {
                    // handle reservation path inside same transaction
                    if (existingReservation.ExpiresAt < DateTime.UtcNow)
                    {
                        throw new BusinessException("Reservation has expired");
                    }

                    coupon = Coupon.Create(userId, existingReservation.OfferId, _couponCodeGenerator.Generate());
                    _reservationRepository.Delete(existingReservation);
                }
                else
                {
                    // directly using internal PurchaseFromReservationAsync may cause transaction issues??!!
                    // direct purchase path
                    offer.RemainingCoupons--;
                    coupon = Coupon.Create(userId, offerId, _couponCodeGenerator.Generate());
                }

                _couponRepository.Add(coupon);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return coupon.Adapt<CouponResponse>();
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        // finish purchase from reserved coupon under users id, coupon count already dedycted
        public async Task<CouponResponse> PurchaseFromReservationAsync(Guid reservationId, CancellationToken cancellationToken)
        {
            _authorization.EnsureAuthenticated();
            var userId = _currentUser.UserId!.Value;

            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var reservation = await _reservationRepository.GetForUpdateAsync(reservationId, cancellationToken);
                if (reservation == null)
                {
                    throw new NotFoundException("Reservation not found");
                }
                if (reservation.UserId != userId)
                {
                    throw new ForbiddenException($"This Reservetion does not belongs to this user");
                }
                if (reservation.ExpiresAt < DateTime.UtcNow)
                {
                    throw new BusinessException("Reservation already expired");
                }

                var coupon = Coupon.Create(userId, reservation.OfferId, _couponCodeGenerator.Generate());

                _couponRepository.Add(coupon);
                _reservationRepository.Delete(reservation);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return coupon.Adapt<CouponResponse>();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
