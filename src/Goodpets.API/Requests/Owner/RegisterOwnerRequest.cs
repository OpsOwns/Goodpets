namespace Goodpets.API.Requests.Owner;

public record RegisterOwnerRequest(string Name, string SureName, string ContactEmail, string Phone, string ZipCode,
    string City, string Street);