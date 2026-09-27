using System.ComponentModel.DataAnnotations;

namespace Rank.Core.DTO.Request.Organizacoes;

public class CreateOrganizacaoRequest
{
    [Required, StringLength(150)]
    public string Nome { get; set; } = string.Empty;

    [StringLength(2048)]
    public string? Logo { get; set; }
}
