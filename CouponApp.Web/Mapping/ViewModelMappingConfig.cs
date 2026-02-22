using Mapster;
using CouponApp.Application.DTOs.Product;
using CouponApp.Web.Models;

namespace CouponApp.Web.Mapping
{
    public class ViewModelMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreateProductViewModel, CreateProductRequest>();

            //config.NewConfig<ProductEditViewModel, UpdateProductRequest>();
            config.NewConfig<ProductResponse, DisplayProductViewModel>()
            .Map(dest => dest.PriceFormatted, src => $"${src.Price:F2}")
            .Map(dest => dest.IsLowStock, src => src.StockQuantity < 10);
        }
    }
}
