using Rank.Core.Auth;
using Rank.Core.DomainEntity;
using Rank.Core.DTO.Request.Users;
using Rank.Core.DTO.Response;
using Rank.Core.Repository;

namespace Rank.Core.Service;

public sealed class UserService(IUserRepository users)
{
    public async Task<UserResponse> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Email = request.Email.Trim().ToLowerInvariant(),
            Name = request.Name.Trim(),
            PasswordHash = PasswordHasher.Hash(request.Password),
            IsActive = true
        };
        var id = await users.CreateAsync(user, cancellationToken);
        return new UserResponse(id, user.Email, user.Name, user.IsActive);
    }

    public async Task<IReadOnlyList<UserResponse>> GetAllAsync(CancellationToken cancellationToken) =>
        (await users.GetAllAsync(cancellationToken)).Select(ToResponse).ToArray();

    public async Task<UserResponse?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        var user = await users.GetByIdAsync(id, cancellationToken);
        return user is null ? null : ToResponse(user);
    }

    public async Task<UserResponse?> UpdateAsync(long id, UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var current = await users.GetByIdAsync(id, cancellationToken);
        if (current is null || !current.IsActive)
            return null;

        var updated = new User
        {
            Id = id,
            Email = request.Email.Trim().ToLowerInvariant(),
            Name = request.Name.Trim(),
            PasswordHash = request.Password is null ? current.PasswordHash : PasswordHasher.Hash(request.Password),
            IsActive = current.IsActive
        };
        return await users.UpdateAsync(updated, cancellationToken) ? ToResponse(updated) : null;
    }

    public Task<bool> DeleteAsync(long id, CancellationToken cancellationToken) =>
        users.DeleteAsync(id, cancellationToken);

    private static UserResponse ToResponse(User user) => new(user.Id, user.Email, user.Name, user.IsActive);
}
