namespace Rank.Core.DomainEntity;

public class Organizacao
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
    public string? Logo { get; set; }
}
