using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Mediator;
using Moq;
using ProjectTemp.Application.Aggregates.Users;
using ProjectTemp.Application.Aggregates.Users.Commands.DeleteUser;
using ProjectTemp.Application.Properties;
using ProjectTemp.Domain.Exceptions;
using ProjectTemp.Domain.UnitTest.Aggregates.Users.Builders;
using Xunit;

namespace ProjectTemp.Application.UnitTest.Aggregates.Users.Commands.DeleteUser;

public class DeleteUserCommandHandlerUnitTest
{
    [Fact]
    public void TestHandle_WhenUserIsSystemEntity_ThrowsException()
    {
        var systemEntityDetectorMock = new Mock<ISystemEntityDetector>();
        var command = DeleteUserCommandBuilder.Build();
        var user = new UserBuilder().Build();
        var commandHandler = new DeleteUserCommandHandlerBuilder()
            .WithSystemEntityDetector(systemEntityDetectorMock.Object)
            .Build();
        var func = new Func<Task>(async () => await ((IRequestHandler<DeleteUserCommand, Unit>) commandHandler)
            .Handle(command, CancellationToken.None));

        systemEntityDetectorMock.Setup(i => i.IsSystemEntity(command.Username)).Returns(true);

        func.Should().Throw<DomainException>()
            .WithMessage(ApplicationResources.User_UnableToDeleteSystemDefined);
    }

    [Fact]
    public void TestHandle_WhenUserDoesNotExit_ThrowsException()
    {
        var systemEntityDetectorMock = new Mock<ISystemEntityDetector>();
        var userWriteRepositoryMock = new Mock<IUserWriteRepository>();
        var command = DeleteUserCommandBuilder.Build();
        var commandHandler = new DeleteUserCommandHandlerBuilder()
            .WithSystemEntityDetector(systemEntityDetectorMock.Object)
            .WithUserWriteRepository(userWriteRepositoryMock.Object)
            .Build();
        var func = new Func<Task>(async () => await ((IRequestHandler<DeleteUserCommand, Unit>) commandHandler)
            .Handle(command, CancellationToken.None));

        systemEntityDetectorMock.Setup(i => i.IsSystemEntity(command.Username)).Returns(false);

        userWriteRepositoryMock
            .Setup(i => i.GetByUsername(command.Username, CancellationToken.None))
            .ReturnsAsync(() => null);

        func.Should().Throw<DomainException>().WithMessage(ApplicationResources.User_UnableToFind);
    }
}