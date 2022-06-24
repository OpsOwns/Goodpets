namespace Goodpets.Application.SeedWork;

public class NotFoundException : BaseException
{
    public NotFoundException() : base("System unable to find value")
    {
    }

    public NotFoundException(string code) : base(code, "System unable to find value")
    {
    }
}