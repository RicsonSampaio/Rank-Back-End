using System.ComponentModel.DataAnnotations;

namespace Rank.Core.DTO.Request.Users;

public class UpdateUserRequest
{
    [Required, EmailAddress, StringLength(320)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(150, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    public string? Password { get; set; }
}
