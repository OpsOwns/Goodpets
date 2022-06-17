namespace Goodpets.API.Configuration.Abstractions;

public interface IExceptionResponseMapper
{
    public ExceptionResponse Map(Exception exception);
}