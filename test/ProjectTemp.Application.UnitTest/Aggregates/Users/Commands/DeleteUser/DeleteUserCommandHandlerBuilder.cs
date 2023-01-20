using System.Threading;
using Moq;
using ProjectTemp.Application.Aggregates.Users;
using ProjectTemp.Application.Aggregates.Users.Commands.DeleteUser;
using ProjectTemp.Domain.UnitTest.Aggregates.Users.Builders;

namespace ProjectTemp.Application.UnitTest.Aggregates.Users.Commands.DeleteUser;

public class DeleteUserCommandHandlerBuilder
{
    private ISystemEntityDetector systemEntityDetector;

    private IUserWriteRepository userWriteRepository;

    public DeleteUserCommandHandlerBuilder()
    {
        var userWriteRepositoryMock = new Mock<IUserWriteRepository>();
        var user = new UserBuilder().Build();

        userWriteRepositoryMock
            .Setup(i => i.GetByUsername(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(user);

        userWriteRepository = userWriteRepositoryMock.Object;
        systemEntityDetector = new Mock<ISystemEntityDetector>().Object;
    }

    public DeleteUserCommandHandler Build()
    {
        return new DeleteUserCommandHandler(userWriteRepository, systemEntityDetector);
    }

    public DeleteUserCommandHandlerBuilder WithUserWriteRepository(IUserWriteRepository value)
    {
        userWriteRepository = value;
        return this;
    }

    public DeleteUserCommandHandlerBuilder WithSystemEntityDetector(ISystemEntityDetector value)
    {
        systemEntityDetector = value;
        return this;
    }
}