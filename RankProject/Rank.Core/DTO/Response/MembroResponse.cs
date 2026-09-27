namespace Rank.Core.DTO.Response;

public record MembroResponse
{
    public int Id { get; set; }

    public int IdColetivo { get; set; }

    public int IdUsuario { get; set; }

    public string? Email { get; set; }

    public string? Nome { get; set; }

    public string? FotoAccount { get; set; }

    public MembroResponse(int id, int idColetivo, int idUsuario, string? email, string? nome, string? fotoAccount)
    {
        Id = id;
        IdColetivo = idColetivo;
        IdUsuario = idUsuario;
        Email = email;
        Nome = nome;
        FotoAccount = fotoAccount;
    }
}
