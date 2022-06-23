namespace Goodpets.Domain.SeedWork;

[Serializable]
public abstract class BaseException : Exception
{
    private const string DefaultParameter = "systemError";
    public string Parameter { get; } = DefaultParameter;

    protected BaseException(string message) : base(message)
    {
    }

    protected BaseException(string parameter, string message) : base(message)
    {
        Parameter = parameter ?? throw new ArgumentNullException(nameof(parameter));
    }
}