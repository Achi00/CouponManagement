using CouponApp.Application.DTOs.Categories;
using CouponApp.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponApp.Application.Interfaces.Repositories
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<IEnumerable<CategoryResponse>> GetAllAsync(CancellationToken cancellationToken);
        Task<Category?> GetByName(string name, CancellationToken cancellationToken);
    }
}
