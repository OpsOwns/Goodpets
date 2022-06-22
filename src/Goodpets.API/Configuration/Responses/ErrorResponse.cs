namespace Goodpets.API.Configuration.Responses;

public record ErrorDetail
{
    public string Code { get; }
    public IEnumerable<string> Messages { get; }

    public ErrorDetail(string code, string message)
    {
        if (message == null)
            throw new ArgumentNullException(nameof(message));
        Code = code ?? throw new ArgumentNullException(nameof(code));
        Messages ??= new List<string> { message };
    }

    public ErrorDetail(string code, IEnumerable<string> messages)
    {
        Code = code ?? throw new ArgumentNullException(nameof(code));
        Messages = messages ?? throw new ArgumentNullException(nameof(messages));
    }
}

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