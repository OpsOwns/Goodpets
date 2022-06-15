namespace Goodpets.API.Configuration;

public interface IExceptionResponseMapper
{
    public ExceptionResponse Map(Exception exception);
}