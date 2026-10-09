using System.Security.Cryptography;

namespace GeoPath.Application;

/// <summary>
/// Hashes passwords for storage and verifies them without retaining plaintext credentials.
/// </summary>
public static class PasswordHasher
{
    /// <summary>Specifies the random salt size in bytes.</summary>
    private const int SaltSize = 16;

    /// <summary>Specifies the derived password hash size in bytes.</summary>
    private const int HashSize = 32;

    /// <summary>Specifies the PBKDF2 work factor used when deriving password hashes.</summary>
    private const int Iterations = 100_000;

    /// <summary>Creates a salted PBKDF2 hash for a password.</summary>
    /// <param name="password">The plaintext password to hash.</param>
    /// <returns>A serialized salt, hash, and iteration count.</returns>
    public static string HashPassword(string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var key = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            HashAlgorithmName.SHA256,
            HashSize);

        return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(key)}:{Iterations}";
    }

    /// <summary>Checks a password against a previously stored salted hash.</summary>
    /// <param name="password">The plaintext password to verify.</param>
    /// <param name="storedHash">The serialized salt, hash, and iteration count.</param>
    /// <returns><see langword="true"/> if the password matches; otherwise, <see langword="false"/>.</returns>
    public static bool VerifyPassword(string password, string storedHash)
    {
        if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(storedHash))
        {
            return false;
        }

        var segments = storedHash.Split(':', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length != 3)
        {
            return false;
        }

        if (!int.TryParse(segments[2], out var iterations) || iterations <= 0)
        {
            return false;
        }

        try
        {
            var salt = Convert.FromBase64String(segments[0]);
            var expectedHash = Convert.FromBase64String(segments[1]);
            var actualHash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256,
                expectedHash.Length);

            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
