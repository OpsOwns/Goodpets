namespace Goodpets.Application.Messages;

public static class CustomResultMessage
{
    public static string Not_Found(string value)
    {
        return $"Not found {value}";
    }
}