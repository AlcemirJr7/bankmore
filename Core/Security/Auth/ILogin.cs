namespace Core.Security.Auth;

public interface ILogin
{
    bool ValidaSenha(string senha, string hash, string salt);
}
