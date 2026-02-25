
namespace CouponApp.Application.DTOs.Search
{
    public class OfferFilterQuery
    {
        public string? SearchQuery { get; set; }
        public Guid? SelectedCategoryId { get; set; }
    }
}
