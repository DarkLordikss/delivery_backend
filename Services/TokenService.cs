using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using food_delivery.Data;
using food_delivery;
using Microsoft.IdentityModel.Tokens;

public class TokenService
{
    private readonly AppDbContext _context;
    private readonly SymmetricSecurityKey _securityKey;

    public TokenService(AppDbContext context)
    {
        _context = context;
        _securityKey = AuthOptions.GetSymmetricSecurityKey();
    }

    public long? GetTokenSeriesByUserId(Guid userId)
    {
        var userPassword = _context.Passwords
            .FirstOrDefault(p => p.UserId == userId);

        return userPassword?.TokenSeries;
    }

    public string? GenerateToken(Guid userId)
    {
        var credentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);

        var tokenSeries = GetTokenSeriesByUserId(userId);

        if (tokenSeries == null)
        {
            return null;
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Version, tokenSeries.ToString()),
        };

        var token = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(AuthOptions.LIFETIME),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
