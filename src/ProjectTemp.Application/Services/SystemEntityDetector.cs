namespace ProjectTemp.Application.Services
{
    public sealed class SystemEntityDetector : ISystemEntityDetector
    {
        public bool IsSystemEntity(string name)
        {
            return name?.StartsWith(ApplicationConstants.SystemEntityIdentifier) ?? false;
        }
    }
}