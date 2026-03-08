using Moq;
using ZedLive.Infrastructure.Data.Context;
using ZedLive.Tests.User.Domain.Contracts.Users.Infra;
using ZedLive.Tests.User.Domain.ValueObjects;
using ZedLive.Tests.User.Infrastructure.data.Context;
using ZedLive.Tests.User.UserRepository;

namespace ZedLive.Tests.User;

public class Startup
{
    private readonly Mock<IUnitOfWork> _work;

    [Fact]
    public void IsAInstance()
    {
        var context = new ZedLiveContextTest();
        var unit = new UnitOfWorkTest(context).Repository<Domain.User.User>();
        var unit2 = new UnitOfWorkTest(context).Repository<Domain.User.User>();

        Assert.Equal(unit, unit2);
    }

    [Fact]
    public void ThrowEmailCreateAUser()
    {
        Assert.Throws<ArgumentException>(() =>
            Domain.User.User.CreateUser("sim", new(""),
                new("adaaaaaa123"))
        );
    }

    [Fact]
    public void ThrowPasswordCreateAUser()
    {
        Assert.Throws<ArgumentException>(() =>
            Domain.User.User.CreateUser("sim", new("adad@gmail.com"),
                new(""))
        );
    }

}