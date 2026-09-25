using Microsoft.Extensions.DependencyInjection;
using Rank.Core.DTO.Request.Users;
using Rank.Core.DTO.Response;
using Rank.Core.Service;

namespace Rank.Application.App;

public class UserApp
{
    private readonly IServiceProvider _provider;

    public UserApp(IServiceProvider provider)
    {
        _provider = provider;
    }

    public Task<UserResponse> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var userService = _provider.GetRequiredService<UserService>();
        return userService.CreateAsync(request, cancellationToken);
    }

    public Task<IReadOnlyList<UserResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        var userService = _provider.GetRequiredService<UserService>();
        return userService.GetAllAsync(cancellationToken);
    }

    public Task<UserResponse?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        var userService = _provider.GetRequiredService<UserService>();
        return userService.GetByIdAsync(id, cancellationToken);
    }

    public Task<UserResponse?> UpdateAsync(long id, UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var userService = _provider.GetRequiredService<UserService>();
        return userService.UpdateAsync(id, request, cancellationToken);
    }

    public Task<bool> DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var userService = _provider.GetRequiredService<UserService>();
        return userService.DeleteAsync(id, cancellationToken);
    }
}
