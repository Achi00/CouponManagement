using CouponApp.Web.Areas.Admin.ViewModels;

namespace CouponApp.Web.Models.Offer
{
    public class OffersIndexViewModel
    {
        public List<OfferListItemViewModel> Offers { get; set; }
        public string? SearchQuery { get; set; }
        public Guid? SelectedCategoryId { get; set; }
        public List<CategoryViewModel> Categories { get; set; }
    }
}
