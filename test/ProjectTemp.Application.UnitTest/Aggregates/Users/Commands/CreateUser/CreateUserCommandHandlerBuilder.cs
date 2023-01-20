using Moq;
using ProjectTemp.Application.Aggregates.Users;
using ProjectTemp.Application.Aggregates.Users.Commands.CreateUser;

namespace ProjectTemp.Application.UnitTest.Aggregates.Users.Commands.CreateUser;

public class CreateUserCommandHandlerBuilder
{
    private ISystemEntityDetector systemEntityDetector;

    public CreateUserCommandHandlerBuilder()
    {
        systemEntityDetector = new Mock<ISystemEntityDetector>().Object;
    }

    public CreateUserCommandHandler Build()
    {
        return new CreateUserCommandHandler(
            new Mock<IUserWriteRepository>().Object,
            systemEntityDetector);
    }

    public CreateUserCommandHandlerBuilder WithSystemEntityDetector(ISystemEntityDetector value)
    {
        systemEntityDetector = value;
        return this;
    }
}