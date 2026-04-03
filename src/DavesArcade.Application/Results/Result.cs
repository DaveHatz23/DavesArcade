namespace DavesArcade.Application.Results;

public record Error(string Code, string Description, ErrorType Type);

public enum ErrorType
{
    Validation,
    Conflict,
    NotFound
}

public class Result<T>
{
    public bool IsSuccess { get; }
    public Error? Error { get; }
    public T? Value { get; }

    private Result(bool isSuccess, T? value, Error? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public static Result<T> Success(T value) => new(true, value, null);

    public static Result<T> NotFound(string code, string description) =>
        new(false, default, new Error(code, description, ErrorType.NotFound));

    public static Result<T> Failure(string code, string description, ErrorType type = ErrorType.Validation) =>
        new(false, default, new Error(code, description, type));

    public static Result<T> Conflict(string code, string description) =>
        new(false, default, new Error(code, description, ErrorType.Conflict));
}