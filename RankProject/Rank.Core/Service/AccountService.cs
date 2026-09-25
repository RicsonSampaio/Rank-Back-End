using Rank.Core.Auth;
using Rank.Core.DTO.Request.Auth;
using Rank.Core.DTO.Response;
using Rank.Core.Repository;

namespace Rank.Core.Service;

public sealed class AccountService(IUserRepository users, ITokenGenerator tokens)
{
    public async Task<TokenResponse?> GenerateTokenAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await users.GetByEmailAsync(request.Email.Trim(), cancellationToken);
        if (user is null || !user.IsActive || !PasswordHasher.Verify(request.Password, user.PasswordHash))
            return null;

        return tokens.Generate(user);
    }
}
