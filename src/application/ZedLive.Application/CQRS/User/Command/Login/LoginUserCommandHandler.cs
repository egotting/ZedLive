using MediatR;
using Microsoft.Extensions.Logging;
using ZedLive.Application.Abstracts;
using ZedLive.Domain.Contracts.Users.Configuration;
using ZedLive.Domain.Contracts.Users.Infra;
using ZedLive.Domain.User.DTO.Response;
using ZedLive.Domain.ValueObjects;

namespace ZedLive.Application.CQRS.User.Command.Login;

public class LoginUserCommandHandler(IUnitOfWork _work, IJwt jwt, ILogger<LoginUserCommandHandler> _logger)
    : ICommandHandler<LoginUserCommand, string>
{
    public async Task<Result<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var repo = _work.Repository<Domain.User.User>();
        var user = await repo.SingleOrDefaultAsync(x =>
            x.Login == request.login && 
            x.Email.Value == request.email &&
            x.Password.Value == request.password, 
            cancellationToken);
        if (!PasswordObject.Verify(user.Password.Value, user.Salt, request.password))
            return Result.Failure<string>(new (Error.InvalidPassword,"Password is incorrect check"));
        return Result.Success(string.Empty);
    }
}