using System.Linq.Expressions;

namespace ZedLive.Domain.Contracts.Users.Infra;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int Id, CancellationToken cancellationToken);

    Task<IEnumerable<T>> GetAllAsync(int take, int size, CancellationToken cancellationToken);

    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, int take, int size,
        CancellationToken cancellationToken);

    Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken,
        bool AsTracking = false);

    Task AddAsync(T entity, CancellationToken cancellationToken);
    void Update(T entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);

    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
    Task<int> CountAsync(CancellationToken cancellationToken);
    Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
}