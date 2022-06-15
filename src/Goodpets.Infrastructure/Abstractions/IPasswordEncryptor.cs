namespace Goodpets.Infrastructure.Abstractions;

public interface IPasswordEncryptor
{
    string Encrypt(string password);
    bool Validate(string password, string securedPassword);
}