namespace ProjectTemp.Domain.Aggregates.Users.Services
{
    public interface IPasswordHashProvider
    {
        string Hash(string password);
    }
}
