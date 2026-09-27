namespace Rank.Core.DTO.Response;

public record ColetivoResponse(
    long Id, string Nome, DateTime DataCriacao, DateTime DataAtualizacao,
    long IdOrganizacao, long IdTipoColetivo, string? Logo);
