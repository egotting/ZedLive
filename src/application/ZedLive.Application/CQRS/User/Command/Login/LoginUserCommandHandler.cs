using MediatR;
using Microsoft.Extensions.Logging;
using ZedLive.Application.Abstracts;
using ZedLive.Domain.Contracts.Users.Configuration;
using ZedLive.Domain.Contracts.Users.Infra;
using ZedLive.Domain.User.DTO.Response;
using ZedLive.Domain.ValueObjects;

namespace ZedLive.Application.CQRS.User.Command.Login;

public class LoginUserCommandHandler(IUnitOfWork _work, IJwt _jwt, ILogger<LoginUserCommandHandler> _logger)
    : ICommandHandler<LoginUserCommand, string>
{
    public async Task<Result<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _work.Repository<Domain.User.User>().SingleOrDefaultAsync(x =>
                x.Login == request.login ||
                x.Email.Value == request.email,
            cancellationToken);
        if (user == null)
        {
            _logger.LogError("{user}, User is null", user);
            return Result.Failure<string>(Error.NullValue);
        }

        if (!PasswordObject.Verify(user.Password.Value, user.Salt,
                request.password))
        {
            _logger.LogError("Password not correct");
            return Result.Failure<string>(Error.InvalidPassword);
        }

        var token = _jwt.Generate(user);
        return Result.Success(token);
    }
}