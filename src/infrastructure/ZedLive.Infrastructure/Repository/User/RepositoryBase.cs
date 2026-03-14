using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ZedLive.Domain.Contracts.Users.Infra;
using ZedLive.Domain.User;
using ZedLive.Infrastructure.Data.Context;

namespace ZedLive.Infrastructure.Repository.User;

public class RepositoryBase<T>(ZedLiveContext _context) : IRepository<T> where T : class
{
    protected DbSet<T> _dbSet = _context.Set<T>();

    public async Task<T?> GetByIdAsync(int Id, CancellationToken cancellationToken)
        => await _dbSet.FindAsync(Id, cancellationToken);

    public async Task<IEnumerable<T>> GetAllAsync(int take, int size, CancellationToken cancellationToken)
        => await _dbSet
            .Skip(take)
            .Take(size)
            .OrderBy(x => x)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate,
        int take,
        int size,
        CancellationToken cancellationToken)
        => await _dbSet.Where(predicate)
            .Skip(take)
            .Take(size)
            .OrderBy(x => x)
            .ToListAsync(cancellationToken);

    public async Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken,
        bool AsTracking = false)
    {
        if (AsTracking)
        {
            return await _dbSet
                .AsTracking()
                .FirstOrDefaultAsync(predicate, cancellationToken);
        }

        return await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken)
        => await _dbSet.AddAsync(entity, cancellationToken);

    public void Update(T entity)
        => _dbSet.Update(entity);

    public void Remove(T entity)
        => _dbSet.Remove(entity);

    public void RemoveRange(IEnumerable<T> entities)
        => _dbSet.RemoveRange(entities);

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        => await _dbSet.AnyAsync(predicate, cancellationToken);

    public async Task<int> CountAsync(CancellationToken cancellationToken)
        => await _dbSet.CountAsync(cancellationToken);

    public async Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        => await _dbSet.CountAsync(predicate, cancellationToken);
}