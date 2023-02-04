using MediatR;
using ProjectTemp.Application.Properties;
using ProjectTemp.Domain.Aggregates.Users.ValueObjects;
using ProjectTemp.Domain.Exceptions;
using ProjectTemp.Domain.ValueObjects;

namespace ProjectTemp.Application.Aggregates.Users.Commands.UpdateUser;

public sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly ISystemEntityDetector systemEntityDetector;

    private readonly IUserWriteRepository userWriteRepository;

    public UpdateUserCommandHandler(
        IUserWriteRepository userWriteRepository,
        ISystemEntityDetector systemEntityDetector)
    {
        this.userWriteRepository = userWriteRepository;
        this.systemEntityDetector = systemEntityDetector;
    }

    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        if (systemEntityDetector.IsSystemEntity(request.CurrentUsername!))
            throw new DomainException(ApplicationResources.User_UnableToUpdateSystemDefined);

        if (systemEntityDetector.IsSystemEntity(request.Username!))
            throw new DomainException(ApplicationResources.User_UsernameCannotStartWithUnderscore);

        var user = await userWriteRepository
            .GetByUsername(request.CurrentUsername!, cancellationToken)
            .ConfigureAwait(false);

        if (user is null)
            throw new DomainException(ApplicationResources.User_UnableToFind);

        user.ChangeUsername(UserUsername.Create(request.Username!));
        user.ChangeDescription(Description.Create(request.Description));

        return Unit.Value;
    }
}