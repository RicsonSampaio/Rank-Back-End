namespace Rank.Core.DomainEntity;

public class User
{
    public long Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string PasswordHash { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public long? IdOrganizacao { get; init; }
    public bool Admin { get; init; }
    public DateTime DataCriacao { get; init; }
    public string? FotoAccount { get; init; }
}
