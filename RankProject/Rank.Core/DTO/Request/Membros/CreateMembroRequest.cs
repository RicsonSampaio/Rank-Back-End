using System.ComponentModel.DataAnnotations;

namespace Rank.Core.DTO.Request.Membros;

public class CreateMembroRequest
{
    [Range(1, int.MaxValue)]
    public int IdColetivo { get; init; }

    [Required, EmailAddress, StringLength(320)]
    public string Email { get; init; } = string.Empty;
}
