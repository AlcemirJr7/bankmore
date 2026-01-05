using Core.Security.Crypt;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

namespace Core.Infrastructure.Security.Crypt;

public sealed class CryptService : ICryptService
{
    private readonly byte[] _key;

    public CryptService(IConfiguration configuration)
    {
        // IMPORTANTE: Armazene essa chave no Azure Key Vault, AWS Secrets Manager, etc.
        var key = configuration["Encryption:Key"] ?? throw new ArgumentNullException("Encryption:Key não configurada");
        _key = Convert.FromBase64String(key);

        if (_key.Length != 32) throw new ArgumentException("A chave deve ter 256 bits (32 bytes)");
    }

    public string DecryptAES(string value)
    {
        if (string.IsNullOrEmpty(value)) return value;

        try
        {
            var fullCipher = Convert.FromBase64String(value);

            using var aes = Aes.Create();
            aes.Key = _key;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            // Extrai o IV do início dos dados
            var iv = new byte[16];
            Array.Copy(fullCipher, 0, iv, 0, iv.Length);
            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor();
            using var ms = new MemoryStream(fullCipher, iv.Length, fullCipher.Length - iv.Length);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var reader = new StreamReader(cs);

            return reader.ReadToEnd();
        }
        catch (CryptographicException ex)
        {
            throw new InvalidOperationException("Erro ao descriptografar dados. Verifique se a chave está correta.", ex);
        }
    }

    public string EncryptAES(string value)
    {
        if (string.IsNullOrEmpty(value)) return value;

        using var aes = Aes.Create();
        aes.Key = _key;
        aes.GenerateIV(); // Gera IV único para cada criptografia
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var encryptor = aes.CreateEncryptor();
        using var ms = new MemoryStream();

        // IMPORTANTE: Armazena o IV junto com os dados criptografados
        ms.Write(aes.IV, 0, aes.IV.Length);

        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var writer = new StreamWriter(cs))
        {
            writer.Write(value);
        }

        return Convert.ToBase64String(ms.ToArray());
    }
}
