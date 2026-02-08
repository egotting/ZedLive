using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore.Storage;
using ZedLive.Tests.User.Domain.Contracts.Users.Infra;
using ZedLive.Tests.User.Infrastructure.data.Context;

namespace ZedLive.Tests.User.UserRepository;

internal class UnitOfWorkTest(ZedLiveContextTest context) : IUnitOfWork
{
    private bool _disposed;
    private IDbContextTransaction? _currentTransaction;

    private readonly ConcurrentDictionary<Type, object> _repositories = new();

    public IRepositoryTest<T> Repository<T>() where T : class
    {
        return (IRepositoryTest<T>)_repositories.GetOrAdd(
            typeof(T),
            _ => new RepositoryTest<T>(context)
        );
    }

    public int SaveChanges() => context.CreateContext().SaveChanges();

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        context.CreateContext().SaveChangesAsync(cancellationToken);

    public async Task BeginTransactionAsync()
    {
        if (_currentTransaction != null) return;
        _currentTransaction = await context.CreateContext().Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_currentTransaction is null) return;
        await _currentTransaction.CommitAsync();
        await _currentTransaction.DisposeAsync();
        _currentTransaction = null;
    }

    public async Task RollbackTransactionAsync()
    {
        if (_currentTransaction == null) return;
        await _currentTransaction.RollbackAsync();
        await _currentTransaction.DisposeAsync();
        _currentTransaction = null;
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            context.CreateContext().Dispose();
            _currentTransaction?.Dispose();
            _disposed = true;
        }

        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        if (!_disposed)
        {
            await context.CreateContext().DisposeAsync();
            if (_currentTransaction != null)
                await _currentTransaction.DisposeAsync();
            _disposed = true;
        }

        GC.SuppressFinalize(this);
    }
}