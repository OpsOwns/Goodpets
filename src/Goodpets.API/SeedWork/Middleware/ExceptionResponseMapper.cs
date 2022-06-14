using Goodpets.Domain.Base.Types;

namespace Goodpets.API.SeedWork.Middleware;

public class ExceptionResponseMapper : IExceptionResponseMapper
{
    private static readonly ConcurrentDictionary<Type, string> Codes = new();

    public ExceptionResponse Map(Exception exception)
    {
        return exception switch
        {
            BusinessException ex => new ExceptionResponse(new ErrorResponse(new Error(GetErrorCode(ex), ex.Message)),
                HttpStatusCode.UnprocessableEntity),
            SecurityTokenExpiredException ex => new ExceptionResponse(
                new ErrorResponse(new Error(GetErrorCode(ex), ex.Message)), HttpStatusCode.Unauthorized),
            _ => new ExceptionResponse(new ErrorResponse(new Error(GetErrorCode(exception), exception.Message)),
                HttpStatusCode.InternalServerError)
        };
    }

    private static string GetErrorCode(object exception)
    {
        var type = exception.GetType();
        return Codes.GetOrAdd(type, type.Name.Underscore().Replace("_exception", string.Empty));
    }

    private record Error(string Code, string Message);

    private record ErrorResponse(params Error[] Errors);
}