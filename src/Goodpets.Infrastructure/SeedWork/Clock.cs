using Goodpets.Application.Abstractions.Time;

namespace Goodpets.Infrastructure.SeedWork;

internal sealed class Clock : IClock
{
    public DateTime Current() => DateTime.UtcNow;
}