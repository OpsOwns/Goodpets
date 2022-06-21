namespace Goodpets.Application.Abstractions;

public interface IPasswordManager
{
    string GeneratePassword(int length);
    string Encrypt(string password);
    bool Validate(string password, string securedPassword);
}