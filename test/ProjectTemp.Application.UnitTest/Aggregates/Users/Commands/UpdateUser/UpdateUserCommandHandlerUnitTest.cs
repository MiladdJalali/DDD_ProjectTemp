using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Mediator;
using Moq;
using ProjectTemp.Application.Aggregates.Users;
using ProjectTemp.Application.Aggregates.Users.Commands.UpdateUser;
using ProjectTemp.Application.Properties;
using ProjectTemp.Domain.Exceptions;
using Xunit;

namespace ProjectTemp.Application.UnitTest.Aggregates.Users.Commands.UpdateUser;

public class UpdateUserCommandHandlerUnitTest
{
    [Fact]
    public void TestHandle_WhenUserIsSystemEntity_ThrowsException()
    {
        var systemEntityDetectorMock = new Mock<ISystemEntityDetector>();
        var command = UpdateUserCommandBuilder.Build();
        var commandHandler = new UpdateUserCommandHandlerBuilder()
            .WithSystemEntityDetector(systemEntityDetectorMock.Object)
            .Build();
        var func = new Func<Task>(async () => await ((IRequestHandler<UpdateUserCommand, Unit>) commandHandler)
            .Handle(command, CancellationToken.None));

        systemEntityDetectorMock.Setup(i => i.IsSystemEntity(command.CurrentUsername)).Returns(true);

        func.Should().Throw<DomainException>()
            .WithMessage(ApplicationResources.User_UnableToUpdateSystemDefined);
    }

    [Fact]
    public void TestHandle_WhenNewUsernameIsSystemEntity_ThrowsException()
    {
        var systemEntityDetectorMock = new Mock<ISystemEntityDetector>();
        var command = UpdateUserCommandBuilder.Build();
        var commandHandler = new UpdateUserCommandHandlerBuilder()
            .WithSystemEntityDetector(systemEntityDetectorMock.Object)
            .Build();
        var func = new Func<Task>(async () => await ((IRequestHandler<UpdateUserCommand, Unit>) commandHandler)
            .Handle(command, CancellationToken.None));

        systemEntityDetectorMock.Setup(i => i.IsSystemEntity(command.CurrentUsername)).Returns(false);

        systemEntityDetectorMock.Setup(i => i.IsSystemEntity(command.Username)).Returns(true);

        func.Should().Throw<DomainException>()
            .WithMessage(ApplicationResources.User_UsernameCannotStartWithUnderscore);
    }

    [Fact]
    public void TestHandle_WhenUserDoesNotExit_ThrowsException()
    {
        var systemEntityDetectorMock = new Mock<ISystemEntityDetector>();
        var userWriteRepositoryMock = new Mock<IUserWriteRepository>();
        var command = UpdateUserCommandBuilder.Build();
        var commandHandler = new UpdateUserCommandHandlerBuilder()
            .WithSystemEntityDetector(systemEntityDetectorMock.Object)
            .WithUserWriteRepository(userWriteRepositoryMock.Object)
            .Build();
        var func = new Func<Task>(async () => await ((IRequestHandler<UpdateUserCommand, Unit>) commandHandler)
            .Handle(command, CancellationToken.None));

        systemEntityDetectorMock.Setup(i => i.IsSystemEntity(command.CurrentUsername)).Returns(false);

        systemEntityDetectorMock.Setup(i => i.IsSystemEntity(command.Username)).Returns(false);

        userWriteRepositoryMock
            .Setup(i => i.GetByUsername(command.CurrentUsername, CancellationToken.None))
            .ReturnsAsync(() => null);

        func.Should().Throw<DomainException>()
            .WithMessage(ApplicationResources.User_UnableToFind);
    }
}