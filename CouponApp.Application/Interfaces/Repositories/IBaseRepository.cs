
namespace CouponApp.Application.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<T?> GetForUpdateAsync(Guid id, CancellationToken cancellationToken);
        void Add(T entity);
        void Delete(T entity);
    }
}
