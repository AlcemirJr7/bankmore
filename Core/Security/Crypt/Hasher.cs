using System.Buffers;
using System.Security.Cryptography;

namespace Core.Security.Crypt;

public class Hasher : IHasher
{
    public HashResult CreateHash(string value)
    {
        byte[] saltBuffer = ArrayPool<byte>.Shared.Rent(CryptConsts.SaltSize);
        byte[] hashBuffer = ArrayPool<byte>.Shared.Rent(CryptConsts.KeySize);

        try
        {
            RandomNumberGenerator.Fill(saltBuffer.AsSpan(0, CryptConsts.SaltSize));

            Rfc2898DeriveBytes.Pbkdf2(
                password: value,
                salt: saltBuffer.AsSpan(0, CryptConsts.SaltSize),
                iterations: CryptConsts.Iterations,
                hashAlgorithm: HashAlgorithmName.SHA256,
                destination: hashBuffer.AsSpan(0, CryptConsts.KeySize)
            );

            var hash = Convert.ToBase64String(hashBuffer, 0, CryptConsts.KeySize);
            var salt = Convert.ToBase64String(saltBuffer, 0, CryptConsts.SaltSize);

            return new HashResult
            {
                Hash = hash,
                Salt = salt
            };
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(saltBuffer, clearArray: true);
            ArrayPool<byte>.Shared.Return(hashBuffer, clearArray: true);
        }
    }
}
