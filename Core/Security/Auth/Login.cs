using Core.Security.Crypt;
using System.Buffers;
using System.Security.Cryptography;

namespace Core.Security.Auth;

public sealed class Login : ILogin
{
    public bool ValidaSenha(string senha, string hash, string salt)
    {
        // Estimar tamanho máximo para saltBytes e hashBytes
        // Base64 expande 3 bytes para 4 chars. Então, 4 chars -> 3 bytes.
        // Tamanho do buffer = (string.Length / 4) * 3
        int maxSaltBytesLength = salt.Length / 4 * 3;
        int maxHashBytesLength = hash.Length / 4 * 3;

        byte[] saltBuffer = ArrayPool<byte>.Shared.Rent(maxSaltBytesLength);
        byte[] hashBuffer = ArrayPool<byte>.Shared.Rent(maxHashBytesLength);
        byte[] computedHashBuffer = ArrayPool<byte>.Shared.Rent(CryptConsts.KeySize);

        try
        {
            if (!Convert.TryFromBase64String(salt, saltBuffer.AsSpan(), out int actualSaltBytesLength) ||
                !Convert.TryFromBase64String(hash, hashBuffer.AsSpan(), out int actualHashBytesLength))
            {
                return false;
            }

            Rfc2898DeriveBytes.Pbkdf2(
                password: senha,
                salt: saltBuffer.AsSpan(0, actualSaltBytesLength),
                iterations: CryptConsts.Iterations,
                hashAlgorithm: HashAlgorithmName.SHA256,
                destination: computedHashBuffer.AsSpan(0, CryptConsts.KeySize)
            );

            return CryptographicOperations.FixedTimeEquals(
                    hashBuffer.AsSpan(0, actualHashBytesLength),
                    computedHashBuffer.AsSpan(0, CryptConsts.KeySize));
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(saltBuffer, clearArray: true);
            ArrayPool<byte>.Shared.Return(hashBuffer, clearArray: true);
            ArrayPool<byte>.Shared.Return(computedHashBuffer, clearArray: true);
        }
    }
}
