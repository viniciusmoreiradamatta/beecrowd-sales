using System.Text.Json.Serialization;

namespace SalesDomain.Abstractions.Response;

public class Result
{
    public EStatusResponse Status { get; set; }
    public string? Message { get; set; }
    public string? Detais { get; set; }

    [JsonIgnore]
    public bool IsSuccess => Status == EStatusResponse.Success;

    public static Result CreateErrorResponse(string message, EStatusResponse status, string? detais = null) => new() { Status = status, Message = message, Detais = detais };

    public static Result CreateErrorResponse(string message) => new() { Status = EStatusResponse.Error, Message = message };

    public static Result CreateSuccessResponse(string message) => new() { Message = message, Status = EStatusResponse.Success };
}

public class Result<T> : Result
{
    [JsonConstructorAttribute]
    protected Result()
    {
    }

    public T Data { get; set; } = default;

    private Result(T data, EStatusResponse status, string message)
    {
        Data = data;
        Status = status;
        Message = message;
    }

    private Result(EStatusResponse status, string message)
    {
        Status = status;
        Message = message;
    }

    public static Result<T> CreateResponse(string message, EStatusResponse status, string? detais = null) => new() { Status = status, Message = message, Detais = detais };

    public static Result<T> CreateErrorResponse(string message, EStatusResponse status) => new(status, message);

    public static Result<T> CreateErrorResponse(string message) => new(EStatusResponse.Error, message);

    public static Result<T> CreateSuccessResponse(T data, string? message = null) => new(data, EStatusResponse.Success, message);
}

public enum EStatusResponse
{
    Success,
    InvalidData,
    NotFound,
    Error
}