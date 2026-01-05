namespace Core.Security.Jwt;

public interface IJwtTokenService
{
    string GenerateToken(string idContaCorrente);
}
