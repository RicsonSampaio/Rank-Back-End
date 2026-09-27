using System.ComponentModel.DataAnnotations;

namespace Rank.Core.DTO.Request.Auth;

public class LoginRequest
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = true)]
    public string Password { get; set; } = string.Empty;
}
