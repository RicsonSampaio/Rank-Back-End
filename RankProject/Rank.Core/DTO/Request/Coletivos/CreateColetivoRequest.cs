using System.ComponentModel.DataAnnotations;
using Rank.Core.Enum;

namespace Rank.Core.DTO.Request.Coletivos;

public class CreateColetivoRequest
{
    [Required, StringLength(150)]
    public string Nome { get; set; } = string.Empty;

    [EnumDataType(typeof(TipoColetivo), ErrorMessage = "Informe um idTipoColetivo válido, de 1 a 5.")]
    public TipoColetivo IdTipoColetivo { get; set; }

    [StringLength(2048)]
    public string? Logo { get; set; }
}
