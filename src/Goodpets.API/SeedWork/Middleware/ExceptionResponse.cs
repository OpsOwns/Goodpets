namespace Goodpets.API.SeedWork.Middleware;

public record ExceptionResponse(object Response, HttpStatusCode HttpStatusCode);