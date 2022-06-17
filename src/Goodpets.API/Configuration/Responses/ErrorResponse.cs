namespace Goodpets.API.Configuration.Responses;

public record ErrorDetail(string Parameter, string Message);

public record ErrorResponse
{
    public IEnumerable<ErrorDetail> Errors { get; }

    public ErrorResponse(IEnumerable<ErrorDetail> errors)
    {
        Errors = errors ?? throw new ArgumentNullException(nameof(errors));
    }

    public ErrorResponse(params ErrorDetail[] errors)
    {
        Errors = errors ?? throw new ArgumentNullException(nameof(errors));
    }
}