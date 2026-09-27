namespace Rank.Core.DTO.Response;

public record OrganizacaoResponse
{
    public int Id { get; set; }

    public string Nome { get; set; }

    public DateTime DataCriacao { get; set; }

    public string? Logo { get; set; }

    public OrganizacaoResponse(int id, string nome, DateTime dataCriacao, string? logo)
    {
        Id = id;
        Nome = nome;
        DataCriacao = dataCriacao;
        Logo = logo;
    }
}
