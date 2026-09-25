using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Rank.Application.App;
using Rank.Core.DTO.Request.Auth;

namespace Rank.WebAPI.Controllers;

[Route("api/[controller]")]
public class AuthController : BaseController
{
    public AuthController(IServiceProvider provider) : base(provider)
    {
    }

    [AllowAnonymous]
    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var accountApp = _provider.GetRequiredService<AccountApp>();
        var token = await accountApp.GenerateTokenAsync(request, cancellationToken);
        return token is null ? Unauthorized(new { message = "Credenciais inválidas." }) : Ok(token);
    }
}
