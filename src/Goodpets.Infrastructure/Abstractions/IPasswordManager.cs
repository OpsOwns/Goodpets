namespace Goodpets.Infrastructure.Abstractions;

public interface IPasswordManager
{
    string GeneratePassword(int length);
    string Encrypt(string password);
    bool Validate(string password, string securedPassword);
}