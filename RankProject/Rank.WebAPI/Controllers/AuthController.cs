using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rank.Application.App;
using Rank.Core.DTO.Request.Auth;

namespace Rank.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController(AccountApp accountApp) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var token = await accountApp.GenerateTokenAsync(request, cancellationToken);
        return token is null ? Unauthorized(new { message = "Credenciais inválidas." }) : Ok(token);
    }
}
