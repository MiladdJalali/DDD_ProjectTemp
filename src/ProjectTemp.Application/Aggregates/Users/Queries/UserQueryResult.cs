namespace ProjectTemp.Application.Aggregates.Users.Queries;

public class UserQueryResult
{
    public Guid Id { get; set; }

    public string Username { get; set; }

    public string? Description { get; set; }

    public string Creator { get; set; }

    public string? Updater { get; set; }

    public DateTimeOffset Created { get; set; }

    public DateTimeOffset? Updated { get; set; }
}