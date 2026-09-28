namespace Rank.Core.DomainEntity;

public class Tarefa
{
    public int Id { get; set; }
    public int IdColetivo { get; set; } = 0;
    public int IdEspaco { get; set; } = 0;
    public int? IdEscopo { get; set; } = 0;
    public int IdStatus { get; set; } = 0;
    public int IdCategoria { get; set; } = 0;
    public int IdUsuarioCriacao { get; set; } = 0;
    public string Titulo { get; set; } = string.Empty;
    public bool Privada { get; set; } = false;
    public string? Descricao { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
    public DateTime? LastDoneDate { get; set; }
    public int? IdTarefaPai { get; set; }
    public string? UserListMarcados { get; set; }
    public string UserListParticipantes { get; set; } = string.Empty;
    public int? IdDocumento { get; set; } = 0;
    public DateTime? PrazoInicial { get; set; }
    public DateTime? PrazoFinal { get; set; }
    public int? IdResponsavel { get; set; } = 0;
    public int? IdFase { get; set; } = 0;
    public int IdRelevancia { get; set; } = 1;
}
