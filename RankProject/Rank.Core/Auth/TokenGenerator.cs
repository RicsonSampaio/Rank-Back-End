using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Rank.Core.DomainEntity;
using Rank.Core.DTO.Response;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Rank.Core.Auth;

public class TokenGenerator
{
    private readonly IServiceProvider _provider;

    public TokenGenerator(IServiceProvider provider)
    {
        _provider = provider;
    }

    public TokenResponse Generate(User user)
    {
        var options = _provider.GetRequiredService<JwtOptions>();
        var expires = DateTimeOffset.UtcNow.AddMinutes(options.ExpirationMinutes);
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString(CultureInfo.InvariantCulture)),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("name", user.Name),
            new Claim("admin", user.Admin ? "true" : "false", ClaimValueTypes.Boolean)
        };
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Key)), SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: options.Issuer,
            audience: options.Audience,
            claims: claims,
            expires: expires.UtcDateTime,
            signingCredentials: credentials);
        // Mantém os campos no payload mesmo quando não há valor no cadastro.
        token.Payload["idOrganizacao"] = user.IdOrganizacao;
        token.Payload["fotoAccount"] = user.FotoAccount;
        return new TokenResponse(new JwtSecurityTokenHandler().WriteToken(token), expires);
    }
}
