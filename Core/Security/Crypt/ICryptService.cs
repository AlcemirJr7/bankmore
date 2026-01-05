namespace Core.Security.Crypt;

public interface ICryptService
{
    string EncryptAES(string value);
    string DecryptAES(string value);
}
