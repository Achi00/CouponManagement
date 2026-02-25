using CouponApp.Domain.Entity;
using CouponApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponApp.Test.Shared
{
    public static class TestData
    {
        public static Offer CreateOffer(
            int coupons = 10,
            Guid? merchantId = null)
        {
            return new Offer
            {
                Id = Guid.NewGuid(),
                MerchantId = merchantId ?? Guid.NewGuid(),
                RemainingCoupons = coupons,
                Status = OfferStatus.Approved,
                CreatedAt = DateTime.UtcNow,
                Title = "Test Offer",
                OriginalPrice = 10,
                DiscountedPrice = 5
            };
        }

        public static Reservation CreateReservation(Guid offerId, Guid userId)
        {
            return new Reservation
            {
                Id = Guid.NewGuid(),
                OfferId = offerId,
                UserId = userId,
                ExpiresAt = DateTime.UtcNow.AddMinutes(30),
                Status = ReservationStatus.Active
            };
        }

        public static Merchant CreateMerchant(Guid? userId = null)
        {
            return new Merchant
            {
                Id = Guid.NewGuid(),
                UserId = userId ?? Guid.NewGuid(),
                BusinessName = "Test Merchant",
                Description = "Test Description"
            };
        }

        public static Category CreateCategory(string name = "Test Category")
        {
            return new Category
            {
                Id = Guid.NewGuid(),
                Name = name
            };
        }

        public static SystemSetting CreateSettings(int reservationMinutes = 30, int editHours = 24)
        {
            return new SystemSetting
            {
                ReservationDurationMinutes = reservationMinutes,
                MerchantEditPeriodHours = editHours
            };
        }
    }
}
