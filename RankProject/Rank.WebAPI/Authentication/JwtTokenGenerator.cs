using Microsoft.IdentityModel.Tokens;
using Rank.Core.Auth;
using Rank.Core.DomainEntity;
using Rank.Core.DTO.Response;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Rank.WebAPI.Authentication;

public sealed class JwtTokenGenerator(JwtOptions options) : ITokenGenerator
{
    public TokenResponse Generate(User user)
    {
        var expires = DateTimeOffset.UtcNow.AddMinutes(options.ExpirationMinutes);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("name", user.Name)
        };
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Key)), SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: options.Issuer,
            audience: options.Audience,
            claims: claims,
            expires: expires.UtcDateTime,
            signingCredentials: credentials);
        return new TokenResponse(new JwtSecurityTokenHandler().WriteToken(token), expires);
    }
}
