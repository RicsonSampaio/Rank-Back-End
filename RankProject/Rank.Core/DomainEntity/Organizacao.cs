namespace Rank.Core.DomainEntity;

public class Organizacao
{
    public int Id { get; init; }
    public string Nome { get; init; } = string.Empty;
    public DateTime DataCriacao { get; init; }
    public string? Logo { get; init; }
}
