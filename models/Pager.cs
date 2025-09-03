namespace AwakenedApi.models;

public class Pager
{
    public int Page { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public string? Search { get; set; }
}