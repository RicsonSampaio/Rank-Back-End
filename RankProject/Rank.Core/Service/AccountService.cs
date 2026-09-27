using Microsoft.Extensions.DependencyInjection;
using Rank.Core.Auth;
using Rank.Core.DTO.Request.Auth;
using Rank.Core.DTO.Response;
using Rank.Core.Repository;

namespace Rank.Core.Service;

public class AccountService
{
    private readonly IServiceProvider _provider;

    public AccountService(IServiceProvider provider)
    {
        _provider = provider;
    }

    public async Task<TokenResponse?> GenerateTokenAsync(LoginRequest request)
    {
        var userRepository = _provider.GetRequiredService<UserRepository>();
        var tokenGenerator = _provider.GetRequiredService<TokenGenerator>();
        var user = await userRepository.GetByEmailAsync(request.Email.Trim());
        if (user is null || !user.IsActive || !PasswordHasher.Verify(request.Password, user.PasswordHash))
            return null;

        return tokenGenerator.Generate(user);
    }
}
