namespace Goodpets.Application.Commands;

public record RegisterCustomer(string Name, string SureName, string ContactEmail, string ZipCode, string City,
    string Street, string PhoneNumber) : ICommand;