namespace Goodpets.Shared.Api.Middleware;

public class ExceptionResponseMapper : IExceptionResponseMapper
{
    private static readonly ConcurrentDictionary<Type, string> Codes = new();

    public ExceptionResponse Map(Exception exception) => exception switch
    {
        BusinessException ex =>
            new ExceptionResponse(new ErrorResponse(new Error(GetErrorCode(ex), ex.Message)),
                HttpStatusCode.UnprocessableEntity),
        _ => new ExceptionResponse(new ErrorResponse(new Error("error", "general error")),
            HttpStatusCode.InternalServerError)
    };

    private record Error(string Code, string Message);

    private record ErrorResponse(params Error[] Errors);

    private static string GetErrorCode(object exception)
    {
        var type = exception.GetType();
        return Codes.GetOrAdd(type, type.Name.Underscore().Replace("_exception", string.Empty));
    }
}