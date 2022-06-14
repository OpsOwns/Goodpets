namespace Goodpets.Infrastructure.SeedWork;

internal sealed class Clock : IClock
{
    public DateTime Current()
    {
        return DateTime.UtcNow;
    }
}