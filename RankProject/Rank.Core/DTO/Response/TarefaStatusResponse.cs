namespace Rank.Core.DTO.Response;

public record TarefaStatusResponse
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
}