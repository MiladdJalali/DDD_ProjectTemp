namespace ProjectTemp.Application;

public class BaseCollectionResult<T>
{
    public IEnumerable<T> Result { get; set; }

    public long TotalCount { get; set; }
}