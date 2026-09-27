namespace Rank.Core.DomainEntity;

public class Organizacao
{
    public long Id { get; init; }
    public string Nome { get; init; } = string.Empty;
    public DateTime DataCriacao { get; init; }
    public string? Logo { get; init; }
}
