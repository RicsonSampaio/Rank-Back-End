using System.ComponentModel.DataAnnotations;

namespace Rank.Core.DTO.Request.Tarefas;

public class UpdateTarefaRequest
{
    public int IdColetivo { get; init; } = 0;
    public int IdEspaco { get; init; } = 0;
    public int? IdEscopo { get; init; } = 0;
    public int IdStatus { get; init; } = 0;
    public int IdCategoria { get; init; } = 0;
    public int IdUsuarioCriacao { get; init; } = 0;
    [Required]
    public string Titulo { get; init; } = string.Empty;
    public bool Privada { get; init; } = false;
    public string? Descricao { get; init; }
    public DateTime? LastDoneDate { get; init; }
    public int? IdTarefaPai { get; init; }
    public string? UserListMarcados { get; init; }
    [Required(AllowEmptyStrings = true)]
    public string UserListParticipantes { get; init; } = string.Empty;
    public int? IdDocumento { get; init; } = 0;
    public DateTime? PrazoInicial { get; init; }
    public DateTime? PrazoFinal { get; init; }
    public int? IdResponsavel { get; init; } = 0;
    public int? IdFase { get; init; } = 0;
    public int IdRelevancia { get; init; } = 1;
}
