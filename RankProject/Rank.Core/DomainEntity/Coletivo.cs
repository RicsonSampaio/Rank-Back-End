namespace Rank.Core.DomainEntity;

public class Coletivo
{
    public long Id { get; init; }
    public string Nome { get; init; } = string.Empty;
    public DateTime DataCriacao { get; init; }
    public DateTime DataAtualizacao { get; init; }
    public long IdOrganizacao { get; init; }
    public long IdTipoColetivo { get; init; }
    public string? Logo { get; init; }
}
