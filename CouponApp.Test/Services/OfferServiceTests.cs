using CouponApp.Application.DTOs.Offers;
using CouponApp.Application.Exceptions;
using CouponApp.Application.Interfaces;
using CouponApp.Application.Interfaces.Repositories;
using CouponApp.Application.Services.Offers;
using CouponApp.Domain.Entity;
using CouponApp.Domain.Enums;
using CouponApp.Test.Shared;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CouponApp.Test.Services
{
    public class OfferServiceTests : ServiceTestBase
    {
        private readonly Mock<IOfferRepository> _offers = new();
        private readonly Mock<IMerchantRepository> _merchants = new();
        private readonly Mock<ISystemSettingsRepository> _settings = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly OfferService _service;

        public OfferServiceTests()
        {
            _service = new OfferService(
                _offers.Object,
                CurrentUser.Object,
                _unitOfWork.Object,
                _settings.Object,
                Authorization.Object,
                _merchants.Object);
        }

        // CreateAsync
        [Fact]
        public async Task CreateAsync_Should_Add_Offer_With_Pending_Status()
        {
            var merchant = TestData.CreateMerchant(UserId);

            _merchants.Setup(x => x.GetByUserIdAsync(UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(merchant);

            var request = new CreateOfferRequest
            {
                Title = "Test Offer",
                TotalCoupons = 10,
                OriginalPrice = 100,
                DiscountedPrice = 50
            };

            await _service.CreateAsync(request, default);

            _offers.Verify(x => x.Add(It.Is<Offer>(o =>
                o.Status == OfferStatus.Pending &&
                o.MerchantId == merchant.Id &&
                o.RemainingCoupons == request.TotalCoupons)), Times.Once);

            _unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_Should_Throw_NotFoundException_When_Merchant_Not_Found()
        {
            _merchants.Setup(x => x.GetByUserIdAsync(UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Merchant?)null);

            Func<Task> act = () => _service.CreateAsync(new CreateOfferRequest(), default);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Merchant not found");
        }

        // DeleteAsync
        [Fact]
        public async Task DeleteAsync_Should_Delete_Offer_When_Merchant_Owns_It()
        {
            var merchant = TestData.CreateMerchant(UserId);
            var offer = TestData.CreateOffer(merchantId: merchant.Id);

            _merchants.Setup(x => x.GetByUserIdAsync(UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(merchant);

            _offers.Setup(x => x.GetForUpdateAsync(offer.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(offer);

            await _service.DeleteAsync(offer.Id, default);

            _offers.Verify(x => x.Delete(offer), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_Should_Throw_ForbiddenException_When_Merchant_Does_Not_Own_Offer()
        {
            var merchant = TestData.CreateMerchant(UserId);
            var offer = TestData.CreateOffer(merchantId: Guid.NewGuid()); // different merchant

            _merchants.Setup(x => x.GetByUserIdAsync(UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(merchant);

            _offers.Setup(x => x.GetForUpdateAsync(offer.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(offer);

            Func<Task> act = () => _service.DeleteAsync(offer.Id, default);

            await act.Should().ThrowAsync<ForbiddenException>();
        }

        [Fact]
        public async Task DeleteAsync_Should_Throw_NotFoundException_When_Offer_Not_Found()
        {
            var merchant = TestData.CreateMerchant(UserId);

            _merchants.Setup(x => x.GetByUserIdAsync(UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(merchant);

            _offers.Setup(x => x.GetForUpdateAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Offer?)null);

            Func<Task> act = () => _service.DeleteAsync(Guid.NewGuid(), default);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Offer not found");
        }

        // UpdateAsync
        [Fact]
        public async Task UpdateAsync_Should_Update_Offer_Within_Edit_Window()
        {
            var merchant = TestData.CreateMerchant(UserId);
            var offer = TestData.CreateOffer(merchantId: merchant.Id);
            offer.CreatedAt = DateTime.UtcNow.AddHours(-1); // created 1hr ago
            var settings = TestData.CreateSettings(editHours: 24); // 24hr window

            _merchants.Setup(x => x.GetByUserIdAsync(UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(merchant);

            _offers.Setup(x => x.GetForUpdateAsync(offer.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(offer);

            _settings.Setup(x => x.GetAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(settings);

            await _service.UpdateAsync(offer.Id, new UpdateOfferRequest(), default);

            _unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_Should_Throw_When_Edit_Period_Expired()
        {
            var merchant = TestData.CreateMerchant(UserId);
            var offer = TestData.CreateOffer(merchantId: merchant.Id);
            // created 48hrs ago
            offer.CreatedAt = DateTime.UtcNow.AddHours(-48);
            // 24 hours window
            var settings = TestData.CreateSettings(editHours: 24);

            _merchants.Setup(x => x.GetByUserIdAsync(UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(merchant);

            _offers.Setup(x => x.GetForUpdateAsync(offer.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(offer);

            _settings.Setup(x => x.GetAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(settings);

            Func<Task> act = () => _service.UpdateAsync(offer.Id, new UpdateOfferRequest(), default);

            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage("Offer can no longer be edited, edit period has expired");
        }

        [Fact]
        public async Task UpdateAsync_Should_Throw_ForbiddenException_When_Merchant_Does_Not_Own_Offer()
        {
            var merchant = TestData.CreateMerchant(UserId);
            var offer = TestData.CreateOffer(merchantId: Guid.NewGuid()); // different merchant

            _merchants.Setup(x => x.GetByUserIdAsync(UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(merchant);

            _offers.Setup(x => x.GetForUpdateAsync(offer.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(offer);

            Func<Task> act = () => _service.UpdateAsync(offer.Id, new UpdateOfferRequest(), default);

            await act.Should().ThrowAsync<ForbiddenException>();
        }
    }
}
