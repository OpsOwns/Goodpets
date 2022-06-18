namespace Goodpets.Infrastructure.Security;

[Serializable]
public class UserException : Exception
{
    private const string DefaultParameter = "systemError";
    public string Parameter { get; } = DefaultParameter;

    public UserException(string message) : base(message) { }

    public UserException(string parameter, string message) : base(message)
    {
        Parameter = parameter ?? throw new ArgumentNullException(nameof(parameter));
    }
}