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

    public Task<UserResponse> CreateAsync(CreateUserRequest request)
    {
        var userService = _provider.GetRequiredService<UserService>();
        return userService.CreateAsync(request);
    }

    public Task<IReadOnlyList<UserResponse>> GetAllAsync()
    {
        var userService = _provider.GetRequiredService<UserService>();
        return userService.GetAllAsync();
    }

    public Task<UserResponse?> GetByIdAsync(int id)
    {
        var userService = _provider.GetRequiredService<UserService>();
        return userService.GetByIdAsync(id);
    }

    public Task<UserResponse?> UpdateAsync(int id, UpdateUserRequest request)
    {
        var userService = _provider.GetRequiredService<UserService>();
        return userService.UpdateAsync(id, request);
    }

    public Task<bool> DeleteAsync(int id)
    {
        var userService = _provider.GetRequiredService<UserService>();
        return userService.DeleteAsync(id);
    }
}
