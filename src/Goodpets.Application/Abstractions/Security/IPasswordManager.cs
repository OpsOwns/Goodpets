namespace Goodpets.Application.Abstractions.Security;

public interface IPasswordManager
{
    string GeneratePassword(int length);
    string Encrypt(string password);
    bool Validate(string password, string securedPassword);
}