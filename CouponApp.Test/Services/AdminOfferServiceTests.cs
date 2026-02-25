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
    public class AdminOfferServiceTests : ServiceTestBase
    {
        private readonly Mock<IOfferRepository> _offers = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly AdminOfferService _service;

        public AdminOfferServiceTests()
        {
            _service = new AdminOfferService(
                _offers.Object,
                _unitOfWork.Object,
                Authorization.Object);
        }

        [Fact]
        public async Task ApproveAsync_Should_Set_Status_To_Approved()
        {
            var offer = TestData.CreateOffer();
            offer.Status = OfferStatus.Pending;

            _offers.Setup(x => x.GetForUpdateAsync(offer.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(offer);

            await _service.ApproveAsync(offer.Id, default);

            offer.Status.Should().Be(OfferStatus.Approved);
            _unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ApproveAsync_Should_Throw_NotFoundException_When_Offer_Not_Found()
        {
            _offers.Setup(x => x.GetForUpdateAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Offer?)null);

            Func<Task> act = () => _service.ApproveAsync(Guid.NewGuid(), default);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task RejectAsync_Should_Set_Status_To_Rejected_With_Reason()
        {
            var offer = TestData.CreateOffer();
            offer.Status = OfferStatus.Pending;

            _offers.Setup(x => x.GetForUpdateAsync(offer.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(offer);

            await _service.RejectAsync(offer.Id, "Poor quality", default);

            offer.Status.Should().Be(OfferStatus.Rejected);
            offer.RejectionReason.Should().Be("Poor quality");
            _unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task RejectAsync_Should_Throw_BusinessException_When_Reason_Is_Empty()
        {
            var offer = TestData.CreateOffer();

            _offers.Setup(x => x.GetForUpdateAsync(offer.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(offer);

            Func<Task> act = () => _service.RejectAsync(offer.Id, "", default);

            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage("Rejection reason is required");
        }

        [Fact]
        public async Task RejectAsync_Should_Throw_NotFoundException_When_Offer_Not_Found()
        {
            _offers.Setup(x => x.GetForUpdateAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Offer?)null);

            Func<Task> act = () => _service.RejectAsync(Guid.NewGuid(), "reason", default);

            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}
