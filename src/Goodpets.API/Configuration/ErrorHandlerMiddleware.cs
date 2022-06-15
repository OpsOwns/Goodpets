namespace Goodpets.API.Configuration;

public class ErrorHandlerMiddleware : IMiddleware
{
    private readonly IExceptionResponseMapper _exceptionResponseMapper;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    public ErrorHandlerMiddleware(IExceptionResponseMapper exceptionResponseMapper,
        ILogger<ErrorHandlerMiddleware> logger)
    {
        _exceptionResponseMapper = exceptionResponseMapper;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            await HandleError(context, exception);
        }
    }

    private async Task HandleError(HttpContext context, Exception exception)
    {
        var errorResponse = _exceptionResponseMapper.Map(exception);

        context.Response.StatusCode = (int)errorResponse.HttpStatusCode;

        var response = errorResponse.Response;

        await context.Response.WriteAsJsonAsync(response);
    }
}