using Rank.Core.Enum;

namespace Rank.Core.DomainEntity;

public class Coletivo
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
    public DateTime DataAtualizacao { get; set; }
    public int IdOrganizacao { get; set; }
    public TipoColetivo IdTipoColetivo { get; set; }
    public string? Logo { get; set; }
}
