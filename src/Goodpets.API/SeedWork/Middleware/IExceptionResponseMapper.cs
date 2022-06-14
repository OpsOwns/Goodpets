namespace Goodpets.Shared.Api.Middleware;

public interface IExceptionResponseMapper
{
    public ExceptionResponse Map(Exception exception);
}