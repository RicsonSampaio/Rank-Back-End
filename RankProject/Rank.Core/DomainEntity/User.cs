namespace Rank.Core.DomainEntity;

public class User
{
    public int Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string PasswordHash { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public int? IdOrganizacao { get; init; }
    public bool Admin { get; init; }
    public DateTime DataCriacao { get; init; }
    public string? FotoAccount { get; init; }
}
