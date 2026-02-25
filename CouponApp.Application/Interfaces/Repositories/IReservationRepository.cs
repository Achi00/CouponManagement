using CouponApp.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponApp.Application.Interfaces.Repositories
{
    public interface IReservationRepository : IBaseRepository<Reservation>
    {
        Task<List<Reservation>> GetAll(CancellationToken cancellationToken);
        Task<List<Reservation>> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<List<Reservation>> GetExpiredActiveReservationsAsync(DateTime now, CancellationToken cancellationToken);
        Task<Reservation?> GetActiveByUserAndOfferAsync(Guid userId, Guid offerId, CancellationToken cancellationToken);
    }
}
