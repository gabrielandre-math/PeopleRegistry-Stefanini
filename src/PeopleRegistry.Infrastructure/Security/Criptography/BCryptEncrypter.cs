using PeopleRegistry.Domain.Security.Cryptography;
using BC = BCrypt.Net.BCrypt;

namespace PeopleRegistry.Infrastructure.Security.Cryptography;

public class BCryptEncrypter : IPasswordEncrypter
{
    public string Encrypt(string password)
    {
        return BC.HashPassword(password);
    }

    public bool Verify(string password, string passwordHash)
    {
        return BC.Verify(password, passwordHash);
    }
}