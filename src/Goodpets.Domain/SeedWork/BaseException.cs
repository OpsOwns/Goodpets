namespace Goodpets.Domain.SeedWork;

[Serializable]
public abstract class BaseException : Exception
{
    private const string DefaultCode = "systemError";
    public string Code { get; } = DefaultCode;

    protected BaseException(string message) : base(message)
    {
    }

    protected BaseException(string code, string message) : base(message)
    {
        Code = code ?? throw new ArgumentNullException(nameof(code));
    }
}