using Microsoft.EntityFrameworkCore;
using CouponApp.Application.Interfaces.Repositories;
using CouponApp.Domain.Entity;
using CouponApp.Domain.Enums;
using CouponApp.Persistence.Contexts;

namespace CouponApp.Infrastructure.Repositories.Reservations
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly DiscountManagementContext _context;

        public ReservationRepository(DiscountManagementContext context)
        {
            _context = context;
        }
        public void Add(Reservation entity)
        {
            _context.Reservations.Add(entity);
        }

        public void Delete(Reservation entity)
        {
            _context.Reservations.Remove(entity);
        }

        public async Task<List<Reservation>> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Reservations
                .AsNoTracking()
                .Include(r => r.Offer)
                .Where(x => x.UserId == userId && x.Status == ReservationStatus.Active)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<List<Reservation>> GetAll(CancellationToken cancellationToken)
        {
            return await _context.Reservations
                .AsNoTracking()
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Reservation?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Reservations
                .AsNoTracking()
                .Include(r => r.Offer)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                .ConfigureAwait(false);
        }

        // check only active reservations if expired
        public async Task<List<Reservation>> GetExpiredActiveReservationsAsync(DateTime now, CancellationToken cancellationToken)
        {
            return await _context.Reservations
                .AsNoTracking()
                .Where(x => x.Status == ReservationStatus.Active && x.ExpiresAt < now)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        // as no tracking version to update
        public async Task<Reservation?> GetForUpdateAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Reservations
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
