using Rank.Core.DTO.Request.Users;
using Rank.Core.DTO.Response;
using Rank.Core.Service;

namespace Rank.Application.App;

public sealed class UserApp(UserService service)
{
    public Task<UserResponse> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken) =>
        service.CreateAsync(request, cancellationToken);

    public Task<IReadOnlyList<UserResponse>> GetAllAsync(CancellationToken cancellationToken) =>
        service.GetAllAsync(cancellationToken);

    public Task<UserResponse?> GetByIdAsync(long id, CancellationToken cancellationToken) =>
        service.GetByIdAsync(id, cancellationToken);

    public Task<UserResponse?> UpdateAsync(long id, UpdateUserRequest request, CancellationToken cancellationToken) =>
        service.UpdateAsync(id, request, cancellationToken);

    public Task<bool> DeleteAsync(long id, CancellationToken cancellationToken) =>
        service.DeleteAsync(id, cancellationToken);
}
