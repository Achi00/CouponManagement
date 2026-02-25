using CouponApp.Application.Exceptions;
using CouponApp.Application.Interfaces;
using CouponApp.Application.Interfaces.Repositories;
using CouponApp.Application.Services;
using CouponApp.Domain.Entity;
using CouponApp.Test.Shared;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;

namespace CouponApp.Test.Services
{
    public class CouponServiceTests : ServiceTestBase
    {
        private readonly Mock<ICouponRepository> _coupons = new();
        private readonly Mock<IOfferRepository> _offers = new();
        private readonly Mock<IReservationRepository> _reservations = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<ICouponCodeGenerator> _codeGenerator = new();
        private readonly CouponService _service;

        public CouponServiceTests()
        {
            _unitOfWork
                .Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<IDbContextTransaction>());

            _codeGenerator
                .Setup(x => x.Generate(10))
                .Returns("TEST-CODE-123");

            _service = new CouponService(
                _coupons.Object,
                _offers.Object,
                _reservations.Object,
                _unitOfWork.Object,
                Authorization.Object,
                CurrentUser.Object,
                _codeGenerator.Object);
        }

        // PurchaseAsync — direct path (no reservation)
        [Fact]
        public async Task PurchaseAsync_Should_Create_Coupon_And_Decrement_When_No_Reservation()
        {
            var offer = TestData.CreateOffer(coupons: 5);

            _offers.Setup(x => x.GetForUpdateAsync(offer.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(offer);

            _reservations.Setup(x => x.GetActiveByUserAndOfferAsync(UserId, offer.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Reservation?)null);

            await _service.PurchaseAsync(offer.Id, default);

            offer.RemainingCoupons.Should().Be(4);
            _coupons.Verify(x => x.Add(It.IsAny<Coupon>()), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task PurchaseAsync_Should_Use_Reservation_And_Not_Decrement_When_Reservation_Exists()
        {
            var offer = TestData.CreateOffer(coupons: 5);
            var reservation = TestData.CreateReservation(offer.Id, UserId);
            reservation.ExpiresAt = DateTime.UtcNow.AddMinutes(10);

            _offers.Setup(x => x.GetForUpdateAsync(offer.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(offer);

            _reservations.Setup(x => x.GetActiveByUserAndOfferAsync(UserId, offer.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(reservation);

            await _service.PurchaseAsync(offer.Id, default);

            offer.RemainingCoupons.Should().Be(5); // not decremented
            _reservations.Verify(x => x.Delete(reservation), Times.Once);
            _coupons.Verify(x => x.Add(It.IsAny<Coupon>()), Times.Once);
        }

        [Fact]
        public async Task PurchaseAsync_Should_Throw_NotFoundException_When_Offer_Not_Found()
        {
            _offers.Setup(x => x.GetForUpdateAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Offer?)null);

            Func<Task> act = () => _service.PurchaseAsync(Guid.NewGuid(), default);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task PurchaseAsync_Should_Throw_BusinessException_When_No_Coupons_Left()
        {
            var offer = TestData.CreateOffer(coupons: 0);

            _offers.Setup(x => x.GetForUpdateAsync(offer.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(offer);

            Func<Task> act = () => _service.PurchaseAsync(offer.Id, default);

            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage("No coupons available");
        }

        [Fact]
        public async Task PurchaseAsync_Should_Throw_When_Reservation_Expired()
        {
            var offer = TestData.CreateOffer(coupons: 5);
            var reservation = TestData.CreateReservation(offer.Id, UserId);
            reservation.ExpiresAt = DateTime.UtcNow.AddMinutes(-5); // expired

            _offers.Setup(x => x.GetForUpdateAsync(offer.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(offer);

            _reservations.Setup(x => x.GetActiveByUserAndOfferAsync(UserId, offer.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(reservation);

            Func<Task> act = () => _service.PurchaseAsync(offer.Id, default);

            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage("Reservation has expired");
        }

        // PurchaseFromReservationAsync
        [Fact]
        public async Task PurchaseFromReservationAsync_Should_Create_Coupon_And_Delete_Reservation()
        {
            var reservation = TestData.CreateReservation(Guid.NewGuid(), UserId);
            reservation.ExpiresAt = DateTime.UtcNow.AddMinutes(10);

            _reservations.Setup(x => x.GetForUpdateAsync(reservation.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(reservation);

            await _service.PurchaseFromReservationAsync(reservation.Id, default);

            _coupons.Verify(x => x.Add(It.IsAny<Coupon>()), Times.Once);
            _reservations.Verify(x => x.Delete(reservation), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task PurchaseFromReservationAsync_Should_Throw_NotFoundException_When_Reservation_Not_Found()
        {
            _reservations.Setup(x => x.GetForUpdateAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Reservation?)null);

            Func<Task> act = () => _service.PurchaseFromReservationAsync(Guid.NewGuid(), default);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task PurchaseFromReservationAsync_Should_Throw_ForbiddenException_When_Wrong_User()
        {
            var reservation = TestData.CreateReservation(Guid.NewGuid(), Guid.NewGuid()); // different userId
            reservation.ExpiresAt = DateTime.UtcNow.AddMinutes(10);

            _reservations.Setup(x => x.GetForUpdateAsync(reservation.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(reservation);

            Func<Task> act = () => _service.PurchaseFromReservationAsync(reservation.Id, default);

            await act.Should().ThrowAsync<ForbiddenException>();
        }

        [Fact]
        public async Task PurchaseFromReservationAsync_Should_Throw_When_Reservation_Expired()
        {
            var reservation = TestData.CreateReservation(Guid.NewGuid(), UserId);
            reservation.ExpiresAt = DateTime.UtcNow.AddMinutes(-5);

            _reservations.Setup(x => x.GetForUpdateAsync(reservation.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(reservation);

            Func<Task> act = () => _service.PurchaseFromReservationAsync(reservation.Id, default);

            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage("Reservation already expired");
        }
    }
}
