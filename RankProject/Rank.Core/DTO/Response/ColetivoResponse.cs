namespace Rank.Core.DTO.Response;

public record ColetivoResponse(
    int Id, string Nome, DateTime DataCriacao, DateTime DataAtualizacao,
    int IdOrganizacao, int IdTipoColetivo, string? Logo);
