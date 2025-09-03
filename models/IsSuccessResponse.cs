namespace AwakenedApi.models;

public class IsSuccessResponse<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public T Data { get; set; } 
}

public class IsSuccessResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
}