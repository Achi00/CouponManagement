using CouponApp.Application.DTOs.Categories;
using CouponApp.Application.DTOs.Offers;

namespace CouponApp.Web.Models.Home
{
    public class HomeViewModel
    {
        public IReadOnlyList<OfferResponse> Offers { get; set; }
        public IEnumerable<CategoryResponse> Categories { get; set; } = new List<CategoryResponse>();
        public Guid? SelectedCategoryId { get; set; }
        public string? SearchQuery { get; set; }
    }
}
