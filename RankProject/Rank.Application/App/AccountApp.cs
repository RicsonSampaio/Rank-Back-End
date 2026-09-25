using Rank.Core.DTO.Request.Auth;
using Rank.Core.DTO.Response;
using Rank.Core.Service;

namespace Rank.Application.App;

public sealed class AccountApp(AccountService service)
{
    public Task<TokenResponse?> GenerateTokenAsync(LoginRequest request, CancellationToken cancellationToken) =>
        service.GenerateTokenAsync(request, cancellationToken);
}
