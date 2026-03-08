using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore.Storage;
using ZedLive.Domain.Contracts.Users.Infra;
using ZedLive.Infrastructure.Data.Context;

namespace ZedLive.Infrastructure.Repository.User;

public class UnitOfWork(ZedLiveContext context) : IUnitOfWork
{
    private bool _disposed;
    private IDbContextTransaction? _currentTransaction;

    private readonly ConcurrentDictionary<Type, object> _repositories = new();

    public IRepository<T> Repository<T>() where T : class
    {
        return (IRepository<T>)_repositories.GetOrAdd(
            typeof(T),
            _ => new RepositoryBase<T>(context)
        );
    }

    public int SaveChanges() => context.SaveChanges();

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        context.SaveChangesAsync(cancellationToken);

    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        if (_currentTransaction != null) return;
        _currentTransaction = await context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        if (_currentTransaction is null) return;
        await _currentTransaction.CommitAsync(cancellationToken);
        await _currentTransaction.DisposeAsync();
        _currentTransaction = null;
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken)
    {
        if (_currentTransaction == null) return;
        await _currentTransaction.RollbackAsync(cancellationToken);
        await _currentTransaction.DisposeAsync();
        _currentTransaction = null;
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            context.Dispose();
            _currentTransaction?.Dispose();
            _disposed = true;
        }

        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        if (!_disposed)
        {
            await context.DisposeAsync();
            if (_currentTransaction != null)
                await _currentTransaction.DisposeAsync();
            _disposed = true;
        }

        GC.SuppressFinalize(this);
    }
}