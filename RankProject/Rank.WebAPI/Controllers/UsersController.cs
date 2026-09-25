using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Rank.Application.App;
using Rank.Core.DTO.Request.Users;
using Rank.Core.Service;

namespace Rank.WebAPI.Controllers;

[Route("api/[controller]")]
public class UsersController : BaseController
{
    public UsersController(IServiceProvider provider) : base(provider)
    {
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var userApp = _provider.GetRequiredService<UserApp>();
            var user = await userApp.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }
        catch (EmailAlreadyInUseException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var userApp = _provider.GetRequiredService<UserApp>();
        return Ok(await userApp.GetAllAsync(cancellationToken));
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var userApp = _provider.GetRequiredService<UserApp>();
        var user = await userApp.GetByIdAsync(id, cancellationToken);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var userApp = _provider.GetRequiredService<UserApp>();
            var user = await userApp.UpdateAsync(id, request, cancellationToken);
            return user is null ? NotFound() : Ok(user);
        }
        catch (EmailAlreadyInUseException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        var userApp = _provider.GetRequiredService<UserApp>();
        return await userApp.DeleteAsync(id, cancellationToken) ? NoContent() : NotFound();
    }
}
