using CouponApp.Application.Exceptions;
using CouponApp.Application.Interfaces;
using CouponApp.Application.Interfaces.Repositories;
using CouponApp.Application.Services;
using CouponApp.Domain.Entity;
using CouponApp.Domain.Enums;
using CouponApp.Test.Shared;
using FluentAssertions;
using Moq;

namespace CouponApp.Test.Services
{
    public class ReservationServiceTests : ServiceTestBase
    {
        private readonly Mock<IReservationRepository> _reservations = new();
        private readonly Mock<IOfferRepository> _offers = new();
        private readonly Mock<ISystemSettingsRepository> _settings = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly ReservationService _service;

        public ReservationServiceTests()
        {
            // default: no existing reservations
            _reservations
                .Setup(x => x.GetActiveByUserIdAsync(UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Reservation>());

            // default: settings with 30 min duration
            _settings
                .Setup(x => x.GetAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SystemSetting { ReservationDurationMinutes = 30 });

            _service = new ReservationService(
                _reservations.Object,
                _offers.Object,
                _settings.Object,
                Authorization.Object,
                CurrentUser.Object,
                _unitOfWork.Object);
        }

        [Fact]
        public async Task CreateAsync_Should_Decrement_RemainingCoupons_And_Add_Reservation()
        {
            var offer = TestData.CreateOffer(coupons: 5);
            offer.Status = OfferStatus.Approved;

            _offers.Setup(x => x.GetForUpdateAsync(offer.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(offer);

            await _service.CreateAsync(offer.Id, default);

            offer.RemainingCoupons.Should().Be(4);
            _reservations.Verify(x => x.Add(It.Is<Reservation>(r =>
                r.UserId == UserId &&
                r.OfferId == offer.Id &&
                r.Status == ReservationStatus.Active)), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_Should_Throw_When_Already_Has_Active_Reservation_For_Offer()
        {
            var offer = TestData.CreateOffer();
            var existing = TestData.CreateReservation(offer.Id, UserId);

            _reservations
                .Setup(x => x.GetActiveByUserIdAsync(UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Reservation> { existing });

            Func<Task> act = () => _service.CreateAsync(offer.Id, default);

            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage("You already have an active reservation for this offer");
        }

        [Fact]
        public async Task CreateAsync_Should_Throw_NotFoundException_When_Offer_Not_Found()
        {
            _offers.Setup(x => x.GetForUpdateAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Offer?)null);

            Func<Task> act = () => _service.CreateAsync(Guid.NewGuid(), default);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task CreateAsync_Should_Throw_When_Offer_Not_Approved()
        {
            var offer = TestData.CreateOffer();
            offer.Status = OfferStatus.Pending;

            _offers.Setup(x => x.GetForUpdateAsync(offer.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(offer);

            Func<Task> act = () => _service.CreateAsync(offer.Id, default);

            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage("Offer is not available");
        }

        [Fact]
        public async Task CreateAsync_Should_Throw_When_No_Coupons_Left()
        {
            var offer = TestData.CreateOffer(coupons: 0);
            offer.Status = OfferStatus.Approved;

            _offers.Setup(x => x.GetForUpdateAsync(offer.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(offer);

            Func<Task> act = () => _service.CreateAsync(offer.Id, default);

            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage("No coupons available");
        }

        [Fact]
        public async Task CancelAsync_Should_Restore_Coupon_Count_And_Delete_Reservation()
        {
            var offer = TestData.CreateOffer(coupons: 3);
            var reservation = TestData.CreateReservation(offer.Id, UserId);

            _reservations.Setup(x => x.GetForUpdateAsync(reservation.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(reservation);

            _offers.Setup(x => x.GetForUpdateAsync(offer.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(offer);

            await _service.CancelAsync(reservation.Id, default);

            offer.RemainingCoupons.Should().Be(4); // restored
            _reservations.Verify(x => x.Delete(reservation), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CancelAsync_Should_Throw_NotFoundException_When_Reservation_Not_Found()
        {
            _reservations.Setup(x => x.GetForUpdateAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Reservation?)null);

            Func<Task> act = () => _service.CancelAsync(Guid.NewGuid(), default);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task CancelAsync_Should_Throw_When_Reservation_Belongs_To_Different_User()
        {
            var reservation = TestData.CreateReservation(Guid.NewGuid(), Guid.NewGuid()); // different user

            _reservations.Setup(x => x.GetForUpdateAsync(reservation.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(reservation);

            Func<Task> act = () => _service.CancelAsync(reservation.Id, default);

            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage("This reservation does not belong to you");
        }
    }
}
