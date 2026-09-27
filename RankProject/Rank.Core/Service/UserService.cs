using Microsoft.Extensions.DependencyInjection;
using Rank.Core.Auth;
using Rank.Core.DomainEntity;
using Rank.Core.DTO.Request.Users;
using Rank.Core.DTO.Response;
using Rank.Core.Repository;

namespace Rank.Core.Service;

public class UserService
{
    private readonly IServiceProvider _provider;

    public UserService(IServiceProvider provider)
    {
        _provider = provider;
    }

    public async Task<UserResponse> CreateAsync(CreateUserRequest request)
    {
        var userRepository = _provider.GetRequiredService<UserRepository>();
        var user = new User
        {
            Email = request.Email.Trim().ToLowerInvariant(),
            Name = request.Name.Trim(),
            PasswordHash = PasswordHasher.Hash(request.Password),
            IsActive = true,
            DataCriacao = DateTime.UtcNow
        };
        var created = await userRepository.CreateAsync(user).ConfigureAwait(false);
        return ToResponse(created);
    }

    public async Task<IReadOnlyList<UserResponse>> GetAllAsync()
    {
        var userRepository = _provider.GetRequiredService<UserRepository>();
        return (await userRepository.GetAllAsync()).Select(ToResponse).ToArray();
    }

    public async Task<UserResponse?> GetByIdAsync(long id)
    {
        var userRepository = _provider.GetRequiredService<UserRepository>();
        var user = await userRepository.GetByIdAsync(id);
        return user is null ? null : ToResponse(user);
    }

    public async Task<UserResponse?> UpdateAsync(long id, UpdateUserRequest request)
    {
        var userRepository = _provider.GetRequiredService<UserRepository>();
        var current = await userRepository.GetByIdAsync(id);
        if (current is null || !current.IsActive)
            return null;

        var updated = new User
        {
            Id = id,
            Email = request.Email.Trim().ToLowerInvariant(),
            Name = request.Name.Trim(),
            PasswordHash = request.Password is null ? current.PasswordHash : PasswordHasher.Hash(request.Password),
            IsActive = current.IsActive,
            IdOrganizacao = current.IdOrganizacao,
            Admin = current.Admin,
            DataCriacao = current.DataCriacao,
            FotoAccount = current.FotoAccount
        };
        return await userRepository.UpdateAsync(updated) ? ToResponse(updated) : null;
    }

    public Task<bool> DeleteAsync(long id)
    {
        var userRepository = _provider.GetRequiredService<UserRepository>();
        return userRepository.DeleteAsync(id);
    }

    private static UserResponse ToResponse(User user) =>
        new(user.Id, user.Email, user.Name, user.IsActive, user.IdOrganizacao);
}
