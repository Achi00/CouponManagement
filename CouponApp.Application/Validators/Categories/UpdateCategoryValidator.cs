using CouponApp.Application.DTOs.Categories;
using FluentValidation;

namespace CouponApp.Application.Validators.Categories
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryRequest>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}
