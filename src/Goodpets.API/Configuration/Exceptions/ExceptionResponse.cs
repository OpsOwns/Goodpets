namespace Goodpets.API.Configuration.Exceptions;

public record ExceptionResponse(object Response, HttpStatusCode HttpStatusCode);