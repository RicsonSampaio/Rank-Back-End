namespace Rank.Core.DomainEntity;

public class Membro
{
    public int Id { get; set; }
    public int IdColetivo { get; set; }
    public int IdUsuario { get; set; }
    public string? Email { get; set; }
    public string? Nome { get; set; }
    public string? FotoAccount { get; set; }
}
