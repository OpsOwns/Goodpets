namespace Goodpets.API.Configuration.Mapper;

public class ExceptionResponseMapper : IExceptionResponseMapper
{
    private static readonly ConcurrentDictionary<Type, string> Codes = new();

    public ExceptionResponse Map(Exception exception)
    {
        return exception switch
        {
            BusinessException ex => new ExceptionResponse(
                new ErrorResponse(new ErrorDetail(GetErrorCode(ex), ex.Message)),
                HttpStatusCode.UnprocessableEntity),
            SecurityTokenExpiredException ex => new ExceptionResponse(
                new ErrorResponse(new ErrorDetail(GetErrorCode(ex), ex.Message)),
                HttpStatusCode.Unauthorized),
            UserException ex => new ExceptionResponse(new ErrorResponse(new ErrorDetail(ex.Parameter, ex.Message)),
                HttpStatusCode.Conflict),
            EmailException ex => new ExceptionResponse(new ErrorResponse(new ErrorDetail(GetErrorCode(ex), ex.Message)),
                HttpStatusCode.Conflict),
            ArgumentNullException ex => new ExceptionResponse(
                new ErrorResponse(new ErrorDetail("NullCheck", ex.Message)), HttpStatusCode.Conflict),
            _ => new ExceptionResponse(
                new ErrorResponse(new ErrorDetail(GetErrorCode(exception), exception.Message)),
                HttpStatusCode.InternalServerError)
        };
    }

    private static string GetErrorCode(object exception)
    {
        var type = exception.GetType();
        return Codes.GetOrAdd(type, type.Name);
    }
}