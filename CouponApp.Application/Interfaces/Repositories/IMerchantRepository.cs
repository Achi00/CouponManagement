using CouponApp.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponApp.Application.Interfaces.Repositories
{
    public interface IMerchantRepository : IBaseRepository<Merchant>
    {
        Task<Merchant?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<Merchant?> GetForUpdateByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<bool> ExistsForUserAsync(Guid userId, CancellationToken cancellationToken);
    }
}
