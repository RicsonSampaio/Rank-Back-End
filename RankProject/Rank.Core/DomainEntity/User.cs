namespace Rank.Core.DomainEntity;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int? IdOrganizacao { get; set; }
    public bool Admin { get; set; }
    public DateTime DataCriacao { get; set; }
    public string? FotoAccount { get; set; }
}
