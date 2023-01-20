namespace ProjectTemp.Infrastructure;

public interface IUserDescriptor
{
    Guid GetId();

    string GetClient();

    string GetClientAddress();
}