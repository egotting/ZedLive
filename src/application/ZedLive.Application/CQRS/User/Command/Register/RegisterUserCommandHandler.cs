using Microsoft.Extensions.Logging;
using ZedLive.Application.Abstracts;
using ZedLive.Domain.Contracts.Users.Configuration;
using ZedLive.Domain.Contracts.Users.Infra;
using ZedLive.Domain.ValueObjects;

namespace ZedLive.Application.CQRS.User.Command.Register;

public class RegisterUserCommandHandler(
    IUnitOfWork _unit,
    IJwtStream _stream,
    ILogger<RegisterUserCommandHandler> _logger)
    : ICommandHandler<RegisterUserCommand>
{
    public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        #region Query

        var exists = await _unit.Repository<Domain.User.User>()
            .ExistsAsync(x => x.Email.Value == request.email || x.Login == request.login, cancellationToken);

        #endregion Query

        if (exists)
        {
            _logger.LogWarning("User already register: {user}", exists);
            return Result.Failure(Error.AlreadyRegister);
        }

        if (!string.Equals(request.password, request.confirmPassword))
        {
            _logger.LogWarning("User {request.email} try to register but dont repeat the same password", request.email);
            return Result.Failure(Error.IncorrectPassword);
        }

        try
        {
            var encrypt = PasswordObject.Hash(request.password);
            var user = Domain.User.User.CreateUser(
                request?.login,
                request.email,
                encrypt.password,
                encrypt.salt,
                string.Empty
            );
            string streamkey = _stream.Generate(user);
            user.StreamKey = streamkey;

            await _unit.Repository<Domain.User.User>().AddAsync(user, cancellationToken);
            await _unit.SaveChangesAsync(cancellationToken);
            await _unit.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception _)
        {
            _logger.LogError("Error:  {_}", _);
            await _unit.RollbackTransactionAsync(cancellationToken);
            return Result.Failure(new Error("exception.error", ""));
        }

        return Result.Success();
    }
}