namespace Rank.Core.DomainEntity;

public class User
{
    public long Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string PasswordHash { get; init; } = string.Empty;
    public bool IsActive { get; init; }
}
