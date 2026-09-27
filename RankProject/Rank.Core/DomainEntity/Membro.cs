namespace Rank.Core.DomainEntity;

public class Membro
{
    public int Id { get; init; }
    public int IdColetivo { get; init; }
    public int IdUsuario { get; init; }
    public string? Email { get; init; }
    public string? Nome { get; init; }
    public string? FotoAccount { get; init; }
}
