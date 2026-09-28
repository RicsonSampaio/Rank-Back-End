using System.Security.Cryptography;

namespace Rank.Core.Auth;

public static class PasswordHasher
{
    private const int Iterations = 210_000;
    private const int SaltLength = 16;
    private const int HashLength = 32;

    public static string Hash(string password)
    {
        ArgumentNullException.ThrowIfNull(password);
        var salt = RandomNumberGenerator.GetBytes(SaltLength);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, HashLength);
        return $"pbkdf2-sha256${Iterations}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
    }

    public static bool Verify(string password, string? storedHash)
    {
        if (password is null || string.IsNullOrEmpty(storedHash))
            return false;

        var parts = storedHash.Split('$');
        if (parts.Length != 4 || parts[0] != "pbkdf2-sha256" ||
            !int.TryParse(parts[1], out var iterations) || iterations < 100_000 || iterations > 1_000_000)
            return false;

        try
        {
            var salt = Convert.FromBase64String(parts[2]);
            var expected = Convert.FromBase64String(parts[3]);
            if (salt.Length != SaltLength || expected.Length != HashLength)
                return false;

            var actual = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, HashLength);
            return CryptographicOperations.FixedTimeEquals(actual, expected);
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
