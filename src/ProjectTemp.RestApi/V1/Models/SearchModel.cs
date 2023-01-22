namespace ProjectTemp.RestApi.V1.Models;

public class SearchModel
{
    public int PageSize { get; set; } = 25;

    public int PageIndex { get; set; } = 1;
}