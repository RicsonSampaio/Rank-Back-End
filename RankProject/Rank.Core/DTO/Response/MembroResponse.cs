namespace Rank.Core.DTO.Response;

public record MembroResponse(
    int Id, int IdColetivo, int IdUsuario, string? Email, string? Nome, string? FotoAccount);
