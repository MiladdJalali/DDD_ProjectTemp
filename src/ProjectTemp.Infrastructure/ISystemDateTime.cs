namespace ProjectTemp.Infrastructure;

public interface ISystemDateTime
{
    DateTimeOffset UtcNow { get; }
}