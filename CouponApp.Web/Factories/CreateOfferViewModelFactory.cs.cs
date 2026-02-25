using CouponApp.Application.Interfaces.Sercives;
using CouponApp.Web.Models.Offer;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CouponApp.Web.Factories
{
    public class CreateOfferViewModelFactory
    {
        private readonly ICategoryService _categoryService;

        public CreateOfferViewModelFactory(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<CreateOfferViewModel> CreateAsync(CancellationToken cancellationToken)
        {
            var categories = await _categoryService.GetAllAsync(cancellationToken);
            return new CreateOfferViewModel
            {
                Categories = categories.Select(c =>
                    new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
            };
        }
    }
}
