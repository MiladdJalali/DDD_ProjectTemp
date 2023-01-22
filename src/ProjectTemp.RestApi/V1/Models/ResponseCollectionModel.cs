namespace ProjectTemp.RestApi.V1.Models
{
    public class ResponseCollectionModel<T>
        where T : class
    {
        public T[] Values { get; set; }

        public long TotalCount { get; set; }
    }
}