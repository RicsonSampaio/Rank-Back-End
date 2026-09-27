using Rank.Core.Enum;

namespace Rank.Core.DTO.Response;

public record ColetivoResponse
{
    public int Id { get; set; }

    public string Nome { get; set; }

    public DateTime DataCriacao { get; set; }

    public DateTime DataAtualizacao { get; set; }

    public int IdOrganizacao { get; set; }

    public TipoColetivo IdTipoColetivo { get; set; }

    public string? Logo { get; set; }

    public ColetivoResponse(int id, string nome, DateTime dataCriacao, DateTime dataAtualizacao, int idOrganizacao, TipoColetivo idTipoColetivo, string? logo)
    {
        Id = id;
        Nome = nome;
        DataCriacao = dataCriacao;
        DataAtualizacao = dataAtualizacao;
        IdOrganizacao = idOrganizacao;
        IdTipoColetivo = idTipoColetivo;
        Logo = logo;
    }
}
