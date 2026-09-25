using Rank.Core.DomainEntity;

namespace Rank.Core.Repository;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<User?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken);
    Task<long> CreateAsync(User user, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(User user, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken);
}
