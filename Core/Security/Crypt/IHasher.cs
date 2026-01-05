namespace Core.Security.Crypt;

public interface IHasher
{
    HashResult CreateHash(string senha);
}
