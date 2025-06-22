namespace auto_front.Lib;

public class IResponse
{
    public bool Success { get; set; } = true;
    public string? Message { get; set; } = null;
    public object? Data { get; set; } = null;
    public IResponse(bool success = true, object? data = null, string? message = null)
    {
        Data = data;
        Message = message;
        Success = success;
    }
    public IResponse()
    {
    }

    public static IResponse Ok(object? data = null, string? message = null)
    {
        return
        new IResponse
        {
            Success = true,
            Data = data,
            Message = message
        };
    }
    public static IResponse Error(string? message = null)
    {
        return
        new IResponse
        {
            Success = false,
            Message = message
        };
    }

}