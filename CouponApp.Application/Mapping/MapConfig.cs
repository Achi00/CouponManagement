using CouponApp.Application.DTOs.Admin;
using CouponApp.Application.DTOs.Coupons;
using CouponApp.Application.DTOs.Offers;
using CouponApp.Application.DTOs.Product;
using CouponApp.Application.DTOs.Reservations;
using CouponApp.Domain.Entity;
using Mapster;

namespace CouponApp.Application.Mapping
{
    public class MapConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Product, ProductResponse>();

            config.NewConfig<Coupon, CouponResponse>()
                .Map(dest => dest.OfferTitle, src => src.Offer.Title);

            config.NewConfig<Offer, OfferDetailsResponse>()
                .Map(dest => dest.MerchantName, src => src.Merchant.BusinessName)
                .Map(dest => dest.CategoryName, src => src.Category.Name);

            config.NewConfig<Offer, OfferListItemResponse>()
                .Map(dest => dest.CategoryName, src => src.Category.Name);

            config.NewConfig<Reservation, ReservationResponse>()
                .Map(dest => dest.OfferTitle, src => src.Offer.Title);

            config.NewConfig<Offer, AdminOfferResponse>()
                .Map(dest => dest.MerchantName, src => src.Merchant.BusinessName)
                .Map(dest => dest.CategoryName, src => src.Category.Name);
        }
    }
}
