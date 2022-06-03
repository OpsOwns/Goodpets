namespace Goodpets.Shared.Api.Middleware;

public record ExceptionResponse(object Response, HttpStatusCode HttpStatusCode);