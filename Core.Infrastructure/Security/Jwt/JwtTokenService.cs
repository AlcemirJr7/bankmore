using Core.Security.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Core.Infrastructure.Security.Jwt;

public sealed class JwtTokenService : IJwtTokenService
{
    private readonly JwtSettings _settings;
    private static readonly JwtSecurityTokenHandler _tokenHandler = new();
    private readonly byte[] _secretBytes;

    public JwtTokenService(IOptions<JwtSettings> options)
    {
        _settings = options.Value;
        _secretBytes = Encoding.UTF8.GetBytes(_settings.Secret);

        if (_secretBytes.Length < 32)
        {
            throw new ArgumentException("JWT Secret must be at least 32 bytes (256 bits) long for HMAC-SHA256. Please check your JwtSettings configuration.", nameof(options));
        }
    }

    public string GenerateToken(string idContaCorrente)
    {
        var key = new SymmetricSecurityKey(_secretBytes);
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new("idContaCorrente", idContaCorrente),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_settings.ExpirationMinutes),
            signingCredentials: creds
        );

        return _tokenHandler.WriteToken(token);
    }
}
