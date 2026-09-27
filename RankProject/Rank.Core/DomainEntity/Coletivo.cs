namespace Rank.Core.DomainEntity;

public class Coletivo
{
    public int Id { get; init; }
    public string Nome { get; init; } = string.Empty;
    public DateTime DataCriacao { get; init; }
    public DateTime DataAtualizacao { get; init; }
    public int IdOrganizacao { get; init; }
    public int IdTipoColetivo { get; init; }
    public string? Logo { get; init; }
}
