using System.Globalization;
using System.IdentityModel.Tokens.Jwt;

namespace Rank.WebAPI.Authentication;

public class CurrentUser
{
    public bool IsAuthenticated { get; }
    public long Id { get; }
    public string Email { get; } = string.Empty;
    public string UserName { get; } = string.Empty;
    public bool Admin { get; }
    public long? IdOrganizacao { get; }
    public string? FotoAccount { get; }

    public CurrentUser(IServiceProvider provider)
    {
        var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
        var user = httpContextAccessor.HttpContext?.User;
        IsAuthenticated = user?.Identity?.IsAuthenticated == true;
        if (!IsAuthenticated || user is null)
            return;

        if (long.TryParse(user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value,
            NumberStyles.Integer, CultureInfo.InvariantCulture, out var id))
            Id = id;

        Email = user.FindFirst(JwtRegisteredClaimNames.Email)?.Value ?? string.Empty;
        UserName = user.FindFirst("name")?.Value ?? string.Empty;
        Admin = bool.TryParse(user.FindFirst("admin")?.Value, out var admin) && admin;

        if (long.TryParse(user.FindFirst("idOrganizacao")?.Value,
            NumberStyles.Integer, CultureInfo.InvariantCulture, out var idOrganizacao))
            IdOrganizacao = idOrganizacao;

        var fotoAccount = user.FindFirst("fotoAccount")?.Value;
        FotoAccount = string.IsNullOrEmpty(fotoAccount) ? null : fotoAccount;
    }
}
