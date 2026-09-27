using System.ComponentModel.DataAnnotations;

namespace Rank.Core.DTO.Request.Auth;

public class LoginRequest
{
    [Required, EmailAddress]
    public string Email { get; init; } = string.Empty;

    [Required(AllowEmptyStrings = true)]
    public string Password { get; init; } = string.Empty;
}
