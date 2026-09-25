using Microsoft.Extensions.DependencyInjection;
using Rank.Core.DTO.Request.Auth;
using Rank.Core.DTO.Response;
using Rank.Core.Service;

namespace Rank.Application.App;

public class AccountApp
{
    private readonly IServiceProvider _provider;

    public AccountApp(IServiceProvider provider)
    {
        _provider = provider;
    }

    public Task<TokenResponse?> GenerateTokenAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var accountService = _provider.GetRequiredService<AccountService>();
        return accountService.GenerateTokenAsync(request, cancellationToken);
    }
}
