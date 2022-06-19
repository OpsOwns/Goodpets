namespace Goodpets.Application.Commands;

public record RegisterClient(string Name, string SureName, string ContactEmail, string ZipCode, string City,
    string Street, string PhoneNumber) : ICommand;