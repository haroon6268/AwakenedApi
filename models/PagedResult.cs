namespace AwakenedApi.models;
public class PagedResult<T>
{
    public List<T> Items { get; set; } = new List<T>();
    public int TotalCount { get; set; }
    public Pager Pager { get; set; }
}