using CouponApp.Application.DTOs.Categories;

namespace CouponApp.Application.Interfaces.Sercives
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponse>> GetAllAsync(CancellationToken cancellationToken);
        Task CreateAsync(CreateCategoryRequest request, CancellationToken cancellationToken);
        Task UpdateAsync(Guid categoryId, UpdateCategoryRequest request, CancellationToken cancellationToken);
        Task DeleteAsync(Guid categoryId, CancellationToken cancellationToken);
    }
}
