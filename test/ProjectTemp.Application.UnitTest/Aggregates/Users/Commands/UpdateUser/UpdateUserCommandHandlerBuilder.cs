using System.Threading;
using Moq;
using ProjectTemp.Application.Aggregates.Users;
using ProjectTemp.Application.Aggregates.Users.Commands.UpdateUser;
using ProjectTemp.Domain.UnitTest.Aggregates.Users.Builders;

namespace ProjectTemp.Application.UnitTest.Aggregates.Users.Commands.UpdateUser;

public class UpdateUserCommandHandlerBuilder
{
    private ISystemEntityDetector systemEntityDetector;

    private IUserWriteRepository userWriteRepository;

    public UpdateUserCommandHandlerBuilder()
    {
        var userWriteRepositoryMock = new Mock<IUserWriteRepository>();
        var user = new UserBuilder().Build();

        userWriteRepositoryMock
            .Setup(i => i.GetByUsername(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(user);

        userWriteRepository = userWriteRepositoryMock.Object;
        systemEntityDetector = new Mock<ISystemEntityDetector>().Object;
    }

    public UpdateUserCommandHandler Build()
    {
        return new UpdateUserCommandHandler(userWriteRepository, systemEntityDetector);
    }

    public UpdateUserCommandHandlerBuilder WithUserWriteRepository(IUserWriteRepository value)
    {
        userWriteRepository = value;
        return this;
    }

    public UpdateUserCommandHandlerBuilder WithSystemEntityDetector(ISystemEntityDetector value)
    {
        systemEntityDetector = value;
        return this;
    }
}