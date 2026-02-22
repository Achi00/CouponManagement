using CouponApp.Application.Interfaces.Repositories;
using CouponApp.Persistence.Contexts;
using CouponApp.Domain.Entity;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Categories.ToListAsync(cancellationToken);
        }

        public Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public Task<Category?> GetForUpdateAsync(Guid id, CancellationToken cancellationToken)
        {
            return _context.Categories.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }
    }
}
