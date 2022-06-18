namespace Goodpets.API.Configuration.Responses;

public record ErrorDetail
{
    public string Parameter { get; }
    public IEnumerable<string> Messages { get; }

    public ErrorDetail(string parameter, string message)
    {
        if (message == null)
            throw new ArgumentNullException(nameof(message));
        Parameter = parameter ?? throw new ArgumentNullException(nameof(parameter));
        Messages ??= new List<string> { message };
    }

    public ErrorDetail(string parameter, IEnumerable<string> messages)
    {
        Parameter = parameter ?? throw new ArgumentNullException(nameof(parameter));
        Messages = messages ?? throw new ArgumentNullException(nameof(messages));
    }
}

public record ErrorResponse(IEnumerable<ErrorDetail> Errors)
{
    public IEnumerable<ErrorDetail> Errors { get; } = Errors ?? throw new ArgumentNullException(nameof(Errors));
}