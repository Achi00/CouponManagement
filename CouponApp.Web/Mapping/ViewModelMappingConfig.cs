using CouponApp.Application.DTOs.Offers;
using CouponApp.Application.DTOs.Product;
using CouponApp.Web.Models;
using CouponApp.Web.Models.Merchant;
using Mapster;
using System.Data;

namespace CouponApp.Web.Mapping
{
    public class ViewModelMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<EditOfferViewModel, UpdateOfferRequest>()
                .Map(dest => dest.ImageUrl, src => src.ExistingImageUrl);
        }
    }
}
