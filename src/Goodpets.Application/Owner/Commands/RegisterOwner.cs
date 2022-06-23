namespace Goodpets.Application.Owner.Commands;

public record RegisterOwner(string Name, string SureName, string ContactEmail, string ZipCode, string City,
    string Street, string PhoneNumber) : ICommand;