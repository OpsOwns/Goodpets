namespace Goodpets.Domain.SeedWork;

public static class ErrorResultMessages
{
    public static Error NotNullOrEmptyError(string propertyName) =>
        new Error($"{propertyName} can't be null or empty").WithMetadata("ErrorCode", propertyName);

    public static Error WithErrorCode(this Error error, string propertyName)
    {
        if (propertyName == null)
            throw new ArgumentNullException(nameof(propertyName));

        error.WithMetadata("ErrorParameter", propertyName);
        return error;
    }
}