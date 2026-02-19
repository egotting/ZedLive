namespace ZedLive.Tests.User.Domain.Contracts.Users.Infra;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    public IRepositoryTest<T> Repository<T>() where T : class;
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    public Task BeginTransactionAsync();
    public Task CommitTransactionAsync();
    public Task RollbackTransactionAsync();
}