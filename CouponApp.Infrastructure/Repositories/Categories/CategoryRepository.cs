using CouponApp.Application.Interfaces.Repositories;
using CouponApp.Persistence.Contexts;
using CouponApp.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using CouponApp.Application.DTOs.Categories;
using Mapster;

namespace CouponApp.Infrastructure.Repositories.Categories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DiscountManagementContext _context;
        public CategoryRepository(DiscountManagementContext context)
        {
            _context = context;
        }
        public void Add(Category entity)
        {
            _context.Categories.Add(entity);
        }

        public void Delete(Category entity)
        {
            _context.Categories.Remove(entity);
        }

        public async Task<IEnumerable<CategoryResponse>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Categories.AsNoTracking().ProjectToType<CategoryResponse>().OrderBy(c => c.Name).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Category?> GetByName(string name, CancellationToken cancellationToken)
        {
            return await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Name == name, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Category?> GetForUpdateAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id, cancellationToken).ConfigureAwait(false);
        }
    }
}
