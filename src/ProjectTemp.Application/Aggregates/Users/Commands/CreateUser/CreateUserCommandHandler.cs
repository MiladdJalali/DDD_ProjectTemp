﻿using Mediator;
using ProjectTemp.Application.Properties;
using ProjectTemp.Domain.Aggregates.Users;
using ProjectTemp.Domain.Aggregates.Users.ValueObjects;
using ProjectTemp.Domain.Exceptions;
using ProjectTemp.Domain.ValueObjects;

namespace ProjectTemp.Application.Aggregates.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, string>
{
    private readonly IUserWriteRepository userWriteRepository;

    private readonly ISystemEntityDetector systemEntityDetector;

    public CreateUserCommandHandler(
        IUserWriteRepository userWriteRepository,
        ISystemEntityDetector systemEntityDetector)
    {
        this.userWriteRepository = userWriteRepository;
        this.systemEntityDetector = systemEntityDetector;
    }

    ValueTask<string> IRequestHandler<CreateUserCommand, string>.Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        if (systemEntityDetector.IsSystemEntity(request.Username))
            throw new DomainException(ApplicationResources.User_UsernameCannotStartWithUnderscore);

        var user = User.Create(
            UserId.Create(Guid.NewGuid()),
            UserUsername.Create(request.Username),
            UserPassword.Create(request.Password.GetHash()));

        user.ChangeDescription(Description.Create(request.Description));

        userWriteRepository.Add(user);

        return ValueTask.FromResult(request.Username);
    }
}