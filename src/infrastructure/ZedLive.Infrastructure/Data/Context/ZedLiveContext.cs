using Microsoft.EntityFrameworkCore;
using ZedLive.Domain.User;

namespace ZedLive.Infrastructure.Data.Context;

public sealed class ZedLiveContext(DbContextOptions opt) : DbContext(opt)
{

    public DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ZedLiveContext).Assembly);
    }
}