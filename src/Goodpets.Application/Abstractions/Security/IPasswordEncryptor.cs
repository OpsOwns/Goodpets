namespace Goodpets.Application.Abstractions.Security;

public interface IPasswordEncryptor
{
    string Encrypt(string password);
    bool Validate(string password, string securedPassword);
}