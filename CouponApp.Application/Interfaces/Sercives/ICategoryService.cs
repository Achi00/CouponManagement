using CouponApp.Application.DTOs.Categories;

namespace CouponApp.Application.Interfaces.Sercives
{
    public interface ICategoryService
    {
        Task<List<CategoryResponse>> GetAllAsync(CancellationToken cancellationToken);
        Task<Guid> CreateAsync(CreateCategoryRequest request, CancellationToken cancellationToken);
        Task UpdateAsync(Guid categoryId, UpdateCategoryRequest request, CancellationToken cancellationToken);
        Task DeleteAsync(Guid categoryId, CancellationToken cancellationToken);
    }
}
