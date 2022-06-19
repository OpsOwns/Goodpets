namespace Goodpets.Infrastructure;

internal sealed class Clock : IClock
{
    public DateTime Current()
    {
        return DateTime.UtcNow;
    }
}