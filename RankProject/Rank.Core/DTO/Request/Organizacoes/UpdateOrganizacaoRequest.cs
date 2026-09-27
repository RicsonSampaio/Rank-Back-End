using System.ComponentModel.DataAnnotations;

namespace Rank.Core.DTO.Request.Organizacoes;

public class UpdateOrganizacaoRequest
{
    [Required, StringLength(150)]
    public string Nome { get; init; } = string.Empty;

    [StringLength(2048)]
    public string? Logo { get; init; }
}
