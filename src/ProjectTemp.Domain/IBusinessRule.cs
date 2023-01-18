namespace ProjectTemp.Domain
{
    public interface IBusinessRule
    {
        string Message { get; }

        string Details { get; }

        bool IsBroken();
    }
}
